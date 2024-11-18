using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonVisual : MonoBehaviour
{
    public Toggle toggle;
    // Start is called before the first frame update
    void Start()
    {
        toggle.onValueChanged.AddListener(ToggleHighlight);
        // toggle.onClick.AddListener(() => StoryManager.Instance.SetValues(toggle));
    }

    public void ToggleHighlight(bool isOn)
    {
        //check if button is in the selected state
        if(!isOn)
        {
            EventSystem.current.SetSelectedGameObject(null);
            StoryManager.Instance.RemoveValue(toggle);
        }
        else
        {
            StoryManager.Instance.SetValue(toggle);
        }
    }
}
