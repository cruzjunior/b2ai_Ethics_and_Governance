using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResolutionPage : MonoBehaviour
{
    [Serializable]
    public class Resolution
    {
        public string resolutionTittle;
        public string resolutionText;
        public Sprite resolutionImage;
    }

    public List<Resolution> resolutions = new List<Resolution>();
    private Image pageImage;
    private TextMeshProUGUI pageText;
    private TextMeshProUGUI pageTitle;
    // Start is called before the first frame update
    void Start()
    {
        pageImage = transform.Find("Image").GetComponent<Image>();
        pageText = transform.Find("Body").GetComponent<TextMeshProUGUI>();
        pageTitle = transform.Find("Banner").transform.Find("Tittle").GetComponent<TextMeshProUGUI>();
        SetPageContent(StoryManager.Instance.conflictValue);
    }

    void SetPageContent(string value)
    {
        foreach (Resolution resolution in resolutions)
        {
            if (resolution.resolutionTittle == value)
            {
                pageImage.sprite = resolution.resolutionImage;
                pageText.text = resolution.resolutionText;
                pageTitle.text = "Resolution that favours " + resolution.resolutionTittle;
            }
        }
    }
}
