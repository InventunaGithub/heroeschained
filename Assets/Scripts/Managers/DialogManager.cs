using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Author: Mert Karavural
// Date: 11 Oct 2020

public class DialogManager : MonoBehaviour
{
    public GameObject DialogWindowPrefab;
    public GameObject AnswerPrefab;
    public DialogItem TargetDialog;
    public string CanvasName = "Canvas";
    public int LineHeight = 40;

    GameObject dialogPanel;

    public void RenderDialog(DialogItem dialogItem, bool setTitle)
    {
        if(dialogItem.Id == -1)
        {
            Destroy(dialogPanel);
            return;
        }

        dialogPanel.transform.Find("RightPanel/Question").gameObject.GetComponent<Text>().text = dialogItem.Question;

        if (setTitle)
        {
            dialogPanel.transform.Find("Header/Text").gameObject.GetComponent<Text>().text = dialogItem.InAnswerText;
        }

        foreach (Transform child in dialogPanel.transform.Find("RightPanel/AnswerArea"))
        {
            Destroy(child.gameObject);
        }

        int index = 0;
        foreach (var answer in dialogItem.Answers)
        {
            GameObject Answer = Instantiate(AnswerPrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            Answer.transform.SetParent(GameObject.Find("AnswerArea").transform, false);
            Answer.transform.localPosition = new Vector3(0, (dialogItem.Answers.Length / 2) * -LineHeight + (index * LineHeight), 0);
            Answer.transform.Find("Text").gameObject.GetComponent<Text>().text = answer.InAnswerText;

            Answer.GetComponent<Button>().onClick.AddListener(delegate { 
                RenderDialog(answer, false); 
            });

            index = index + 1;
        }
    }

    public void Launch()
    {
        dialogPanel = Instantiate(DialogWindowPrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        dialogPanel.transform.SetParent(GameObject.Find(CanvasName).transform, false);

        RenderDialog(TargetDialog, true);
    }
}
