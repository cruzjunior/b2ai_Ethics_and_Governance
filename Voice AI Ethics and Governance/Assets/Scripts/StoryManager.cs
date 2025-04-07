using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StoryManager : MonoBehaviour
{
    public static StoryManager Instance { get; private set; } // Singleton instance
    public GameObject canvas;
    public GameObject startingPagePrefab; // Assign the starting page prefab in the Inspector
    private GameObject currentPageInstance;
    private Toggle[] toggles = new Toggle[2];
    private string[] values = new string[2];
    private Button nxtBtn;
    private GameObject commentPopup;
    public string conflictValue { get; set; }
    public string[] conflictValues = new string[3];
    public GameObject[] resultPages;
    private int resultPageCounter = 0;
    private Dictionary<string, float> conflictValuesDict = new Dictionary<string, float>();
    private Dictionary<string, int> chosenValuesDict = new Dictionary<string, int>();
    public List<string> valuesList = new List<string>();

    public Dictionary<string, float> GetConflictValues()
    {
        return conflictValuesDict;
    }

    public Dictionary<string, int> GetChosenValues()
    {
        return chosenValuesDict;
    }

    private void Awake()
    {
        // Set up the singleton instance
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keeps the instance across scenes
        }
        else
        {
            Destroy(gameObject); // Ensures only one instance exists
        }
    }

    void Start()
    {
        // Load the starting page. The "false" flag indicates no value selections on this page.
        LoadPage(startingPagePrefab, false);

        // Initialize conflictValuesDict with keys from valuesList and set their initial scores to 0.0f
        foreach (string value in valuesList)
        {
            conflictValuesDict.Add(value, 0.0f);
        }
    }

    public void LoadPage(GameObject pagePrefab, bool isValues = false, bool isConflictValues = false)
    {
        // Destroy current page if it exists
        if (currentPageInstance != null)
        {
            Destroy(currentPageInstance);
        }

        // Instantiate the new page as a child of canvas using the overload that sets the parent automatically
        currentPageInstance = Instantiate(pagePrefab, canvas.transform, false);

        // Cache the StoryNode component to avoid multiple GetComponent calls
        StoryNode storyNode = currentPageInstance.GetComponent<StoryNode>();

        // If this is a Favoured Value page, set the next page and increment the result page counter
        if (currentPageInstance.name == "Favoured Value(Clone)")
        {
            storyNode.options[0].nextPagePrefab = resultPages[resultPageCounter];
            resultPageCounter++;
        }

        // Load available options for the current story node
        storyNode.LoadOptions();

        // If the page requires value selections, reset the toggle/value arrays and disable the Next button initially
        if (isValues)
        {
            ResetValues();
            nxtBtn = currentPageInstance.transform.GetChild(0).transform.GetChild(1).GetComponent<Button>();
            nxtBtn.interactable = false;
        }
    }

    public void SetConflictValue(float value)
    {
        float absValue = Mathf.Abs(value);
        float diff = Mathf.Abs(absValue - 1.0f);

        // Determine the conflict value based on the input value thresholds
        if (value > 0.02f)
        {
            conflictValue = values[0];
            conflictValues[resultPageCounter - 1] = values[0];
            conflictValuesDict[values[0]] += absValue;
            conflictValuesDict[values[1]] += diff;
        }
        else if (value < -0.02f)
        {
            conflictValue = values[1];
            conflictValues[resultPageCounter - 1] = values[1];
            conflictValuesDict[values[0]] += diff;
            conflictValuesDict[values[1]] += absValue;
        }
        else
        {
            conflictValue = "Neutral";
            conflictValues[resultPageCounter - 1] = "Neutral";
            conflictValuesDict[values[0]] += 0.5f;
            conflictValuesDict[values[1]] += 0.5f;
        }

        // Update chosenValuesDict counts for each selected value
        foreach (string s in values)
        {
            if (!chosenValuesDict.ContainsKey(s))
            {
                chosenValuesDict.Add(s, 1);
            }
            else
            {
                chosenValuesDict[s]++;
            }
        }
    }

    public string[] GetValues()
    {
        return values;
    }

    public void SetValue(Toggle value)
    {
        // Store the parent's name to avoid redundant calls
        string parentName = value.gameObject.transform.parent.name;

        // If both toggle slots are already filled, disable the first toggle and update the second slot
        if (toggles[1] != null)
        {
            toggles[0].isOn = false;
            toggles[1] = value;
            values[1] = parentName;
        }
        // If only the first slot is filled, assign the new toggle to the second slot
        else if (toggles[0] != null)
        {
            toggles[1] = value;
            values[1] = parentName;
        }
        // Otherwise, assign the toggle to the first slot
        else
        {
            toggles[0] = value;
            values[0] = parentName;
        }

        SetNextButton();
    }

    public void RemoveValue(Toggle value)
    {
        // If the first toggle is removed but the second exists, shift the second into the first slot
        if (toggles[0] == value && toggles[1] != null)
        {
            toggles[0] = toggles[1];
            toggles[1] = null;
            values[0] = values[1];
            values[1] = null;
        }
        else if (toggles[0] == value)
        {
            toggles[0] = null;
            values[0] = null;
        }
        else if (toggles[1] == value)
        {
            toggles[1] = null;
            values[1] = null;
        }

        SetNextButton();
    }

    public void ResetValues()
    {
        toggles = new Toggle[2];
        values = new string[2];
    }

    public void SetNextButton()
    {
        // Create a set of correct answer toggles from the InfoButtonManager
        HashSet<Toggle> ansToggleSet = new HashSet<Toggle>(InfoButtonManager.Instance.rightAnsBtns);
        // Create a set from the currently selected toggles
        HashSet<Toggle> toggleSet = new HashSet<Toggle>(toggles);

        // If any toggle slot is empty, disable the Next button and exit early
        foreach (Toggle value in toggles)
        {
            if (value == null)
            {
                nxtBtn.interactable = false;
                return;
            }
        }

        int count = 0;
        List<string> valuesList = new List<string>();

        // Check each toggle against the set of correct answers
        foreach (Toggle value in toggles)
        {
            if (ansToggleSet.Contains(value))
            {
                count++;
            }
            else
            {
                // Gather names of incorrect toggles and disable their interactivity
                valuesList.Add(value.gameObject.transform.parent.name);
                value.interactable = false;
            }
        }

        // Deselect and remove any toggles that are not correct
        foreach (Toggle value in toggleSet)
        {
            if (!ansToggleSet.Contains(value))
            {
                value.isOn = false;
                RemoveValue(value);
            }
        }

        // If not all selections are correct, open the comment popup with the incorrect toggle names
        if (count != 2)
        {
            commentPopup.GetComponent<CommentPopUpBehaviour>().OpenPanel(valuesList);
            return;
        }

        nxtBtn.interactable = true;
    }

    public void SetCommentPopup(GameObject commentPopup)
    {
        this.commentPopup = commentPopup;
    }
}
