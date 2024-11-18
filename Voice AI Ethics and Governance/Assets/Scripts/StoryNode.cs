using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryNode : MonoBehaviour
{
    [Serializable]
    public class Option
    {
        public string btnText;
        public GameObject nextPagePrefab;
        public bool isValues;
    }

    public List<Option> options = new List<Option>();
    public GameObject buttonPrefab;
    public GameObject buttonParent; 
    // Start is called before the first frame update

    public void LoadOptions()
    {
        foreach (Option option in options)
        {
            if(option.nextPagePrefab == null)
            {
                Debug.LogError("Next page prefab is not assigned for option: " + option.btnText);
                continue;
            }
            GameObject buttonInstance = Instantiate(buttonPrefab, transform);
            buttonInstance.transform.SetParent(buttonParent.transform);
            buttonInstance.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = option.btnText;
            buttonInstance.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => StoryManager.Instance.LoadPage(option.nextPagePrefab, option.isValues));
            if(gameObject.name == "Favoured Value(Clone)")
            {
                buttonInstance.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => GetComponent<ConflictValuesPage>().SubmitValue());
            }
        }
    }
}
