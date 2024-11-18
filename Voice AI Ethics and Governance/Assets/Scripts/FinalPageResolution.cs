using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FinalPageResolution : MonoBehaviour
{

    public List<TextMeshProUGUI> values = new List<TextMeshProUGUI>();
    // Start is called before the first frame update
    void Start()
    {
        SetValues(StoryManager.Instance.conflictValues);
    }

    public void SetValues(string[] values)
    {
        for (int i = 0; i < values.Length; i++)
        {
            this.values[i].text = values[i];
        }
    }
}
