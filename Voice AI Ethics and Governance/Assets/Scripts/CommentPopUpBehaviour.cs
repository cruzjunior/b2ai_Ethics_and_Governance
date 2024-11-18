using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class CommentPopUpBehaviour : MonoBehaviour
{
    public Button closeButton;
    public Button submitButton;
    public InputField commentInputField;
    public TextMeshProUGUI headerText;
    public GameObject opaqueBckgnd;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
        StoryManager.Instance.SetCommentPopup(gameObject);
    }

    public void OpenPanel(List<string> wrongValues)
    {
        int count = wrongValues.Count;

        if(count == 1)
        {
            headerText.text = "The value <b>" + wrongValues[0] + "</b> is incorrect. Please provide a comment to why you think these values are in conflict. (Opional)";
        }
        else
        {
            string wrongValuesString = "";
            for (int i = 0; i < count; i++)
            {
                if (i == count - 1)
                {
                    wrongValuesString += " and <b>" + wrongValues[i] + "</b>";
                }
                else
                {
                    wrongValuesString += "<b>" + wrongValues[i] + "</b>, ";
                }
            }
            headerText.text = "The values " + wrongValuesString + " are incorrect. Please provide a comment to why you think these values are in conflict. (Opional)";
        }

        gameObject.SetActive(true);
        opaqueBckgnd.SetActive(true);

        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
    }

    public void ClosePanel()
    {
        commentInputField.text = "";
        gameObject.SetActive(false);
        opaqueBckgnd.SetActive(false);
    }
}
