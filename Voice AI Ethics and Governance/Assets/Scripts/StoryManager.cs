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
    public Dictionary<string, float> GetConflictValues()
    {
        return conflictValuesDict;
    }

    public Dictionary<string, int> GetChosenValues()
    {
        return chosenValuesDict;
    }
    public List<string> valuesList = new List<string>();
    // Start is called before the first frame update
    
    private void Awake()
    {
        // Set up the singleton instance
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional: keeps the instance across scenes
        }
        else
        {
            Destroy(gameObject); // Ensures only one instance exists
        }
    }
    void Start()
    {
        LoadPage(startingPagePrefab, false);

        foreach(string value in valuesList)
        {
            conflictValuesDict.Add(value, 0.0f);
        }
    }

    public void LoadPage(GameObject pagePrefab, bool isValues = false, bool isConflictValues = false)
    {
        if (currentPageInstance != null)
        {
            Destroy(currentPageInstance);
        }
        currentPageInstance = Instantiate(pagePrefab, canvas.transform);
        currentPageInstance.transform.SetParent(canvas.transform);
        if(currentPageInstance.name == "Favoured Value(Clone)")
        {
            currentPageInstance.GetComponent<StoryNode>().options[0].nextPagePrefab = resultPages[resultPageCounter];
            resultPageCounter++;
        }
        currentPageInstance.GetComponent<StoryNode>().LoadOptions();

        if(isValues)
        {
            ResetValues();
            nxtBtn = currentPageInstance.transform.GetChild(0).transform.GetChild(1).GetComponent<Button>();
            nxtBtn.interactable = false;
        }
    }

    public void SetConflictValue(float value)
    {
        if(value > 0.02f)
        {
            conflictValue = values[0];
            conflictValues[resultPageCounter - 1] = values[0];
            conflictValuesDict[values[0]] += Mathf.Abs(value);
            conflictValuesDict[values[1]] += Mathf.Abs(Mathf.Abs(value) - 1.0f);
        }
        else if(value < -0.02f)
        {
            conflictValue = values[1];
            conflictValues[resultPageCounter - 1] = values[1];
            conflictValuesDict[values[0]] += Mathf.Abs(Mathf.Abs(value) - 1.0f);
            conflictValuesDict[values[1]] += Mathf.Abs(value);
        }
        else
        {
            conflictValue = "Neutral";
            conflictValues[resultPageCounter - 1] = "Neutral";
            conflictValuesDict[values[0]] += 0.5f;
            conflictValuesDict[values[1]] += 0.5f;
        }

        foreach (string s in values)
        {
            if (!chosenValuesDict.ContainsKey(s))
            {
                chosenValuesDict.Add(s, 1);
                continue;
            }

            chosenValuesDict[s]++;
                    
        }
    }

    public string[] GetValues()
    {
        return values;
    }

    public void SetValue(Toggle value)
    {
        if(toggles[1] != null)
        {
            toggles[0].isOn = false;
            toggles[1] = value;
            values[1] = value.gameObject.transform.parent.name;
        }
        else if(toggles[0] != null)
        {
            toggles[1] = value;
            values[1] = value.gameObject.transform.parent.name;
        }
        else
        {
            toggles[0] = value;
            values[0] = value.gameObject.transform.parent.name;
        }

        SetNextButton();
    }

    public void RemoveValue(Toggle value)
    {
        if(toggles[0] == value && toggles[1] != null)
        {
            toggles[0] = toggles[1];
            toggles[1] = null;
        }
        else if(toggles[0] == value)
        {
            toggles[0] = null;
        }
        else if(toggles[1] == value)
        {
            toggles[1] = null;
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
        HashSet<Toggle> ansToggleSet = new HashSet<Toggle>(InfoButtonManager.Instance.rightAnsBtns);
        HashSet<Toggle> toggleSet = new HashSet<Toggle>(toggles);
        foreach (Toggle value in toggles)
        {
            if(value == null)
            {
                nxtBtn.interactable = false;
                return;
            }
        }

        int count = 0;
        List<String> valuesList = new List<string>();
        foreach (Toggle value in toggles)
        {
            if(ansToggleSet.Contains(value))
            {
                count++;
            }
            else
            {
                valuesList.Add(value.gameObject.transform.parent.name);
                value.interactable = false;
            }
        }

        // deselect the toggles that are not in the right answer
        foreach (Toggle value in toggleSet)
        {
            if(!ansToggleSet.Contains(value))
            {
                value.isOn = false;
            }
        }

        if(count != 2)
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
