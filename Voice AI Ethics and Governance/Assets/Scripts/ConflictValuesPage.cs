using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;

public class ConflictValuesPage : MonoBehaviour
{
    public List<TextMeshProUGUI> values = new List<TextMeshProUGUI>();
    public Slider slider;
    public TextMeshProUGUI prompt;
    private float value = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        SetValues(StoryManager.Instance.GetValues());
    }

    public void SetValues(string[] values)
    {
        HashSet<string> uniqueValues = new HashSet<string>();
        for (int i = 0; i < values.Length; i++)
        {
            this.values[i].text = values[i];
            uniqueValues.Add(values[i]);
        }

        if(uniqueValues.Contains("Autonomy") && uniqueValues.Contains("Safety"))
        {
            prompt.text = "Marvin is worried that Mariella might be in danger, but he doesn’t want to push her into more treatment unnecessarily. Which value should be favored, and to what degree?";
        }
        if(uniqueValues.Contains("Efficiency") && uniqueValues.Contains("Freedom from Bias"))
        {
            prompt.text = "Jane is worried about the age-related bias in her app, but eliminating this bias would seriously impact the efficiency of the company. Which value should be favored, and to what degree?";
        }
        if(uniqueValues.Contains("Accuracy") && uniqueValues.Contains("Freedom from Bias"))
        {
            prompt.text = "The company is choosing between two algorithms: one that is more accurate overall, and one that is less biased overall. Which value should be favored, and to what degree?";
        }

    }

    public void SetFavoriteValue()
    {
        value = slider.value;
    }

    public void SubmitValue()    
    {
        StoryManager.Instance.SetConflictValue(value);
    }
}
