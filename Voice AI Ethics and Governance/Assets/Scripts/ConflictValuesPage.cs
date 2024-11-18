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
    private float value = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        SetValues(StoryManager.Instance.GetValues());
    }

    public void SetValues(string[] values)
    {
        for (int i = 0; i < values.Length; i++)
        {
            this.values[i].text = values[i];
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
