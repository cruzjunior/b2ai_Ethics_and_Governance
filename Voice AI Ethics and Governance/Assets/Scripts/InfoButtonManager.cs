using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class InfoButtonManager : MonoBehaviour
{
    public static InfoButtonManager Instance { get; private set; } // Singleton instance
    [Serializable]
    public class InfoButton
    {
        public Button button;
        public string infoText;
    }

    public List<Toggle> rightAnsBtns = new List<Toggle>();

    public List<InfoButton> infoButtons = new List<InfoButton>();
    // Start is called before the first frame update

    void Awake()
    {
        // Set up the singleton instance
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // Ensures only one instance exists
        }
    }
    void Start()
    {
        foreach (InfoButton infoButton in infoButtons)
        {
            infoButton.button.onClick.AddListener(() => InfoPanel.Instance.OpenPanel(infoButton.infoText));
        }

        foreach (Toggle rightAnsBtn in rightAnsBtns)
        {
            rightAnsBtn.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().color = Color.green;
        }
    }
}
