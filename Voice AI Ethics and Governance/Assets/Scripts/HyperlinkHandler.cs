using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class HyperlinkHandler : MonoBehaviour, IPointerClickHandler
{
    private TextMeshProUGUI textMeshPro;
    private Camera uiCamera;

    void Awake()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();
        uiCamera = Camera.main; // Or assign the UI camera explicitly
    }

    public void OnPointerClick(PointerEventData eventData)
    {

        int linkIndex = TMP_TextUtilities.FindIntersectingLink(
            textMeshPro, eventData.position, uiCamera
        );

        if (linkIndex != -1) // A valid link was clicked
        {
            TMP_LinkInfo linkInfo = textMeshPro.textInfo.linkInfo[linkIndex];
            string linkId = linkInfo.GetLinkID();
            OpenLink(linkId);
        }
    }

    private void OpenLink(string url)
    {
        Debug.Log("Open link: " + url);
        Application.OpenURL(url); // Open the URL in the browser
    }
}
