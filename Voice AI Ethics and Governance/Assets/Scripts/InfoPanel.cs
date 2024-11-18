using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.UIElements;

public class InfoPanel : MonoBehaviour
{
    public static InfoPanel Instance { get; private set; } // Singleton instance
    public TextMeshProUGUI bodyText;
    RectTransform rt;
    string currText;
    private GameObject opaqueBckgnd;

    private void Awake()
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
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
        rt = GetComponent<RectTransform>();

        opaqueBckgnd = GameObject.Find("Panel back");
        opaqueBckgnd.SetActive(false);
    }

    public void OpenPanel(string text)
    {
        if (text == currText)
        {
            ClosePanel();
            return;
        }
        currText = text;
        bodyText.text = text;

        // set to middle of the screen
        rt.localPosition = new Vector3(0, 0, 0);

        gameObject.SetActive(true);
        opaqueBckgnd.SetActive(true);

        LayoutRebuilder.ForceRebuildLayoutImmediate(rt);
    }

    public void ClosePanel()
    {
        currText = "";
        gameObject.SetActive(false);
        opaqueBckgnd.SetActive(false);
    }
}
