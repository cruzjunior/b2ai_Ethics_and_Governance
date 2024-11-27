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
            headerText.text = "We didn't see <b>" + wrongValues[0] + "</b> in conflict in this situation. Feel free to describe how you see them in conflict here. (Opional)";
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
            headerText.text = "We didn't see " + wrongValuesString + " in conflict in this situation. Feel free to describe how you see them in conflict here. (Opional)";
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
