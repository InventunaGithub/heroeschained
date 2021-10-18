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
    public DialogItem TestDialog;
    GameObject dialogPanel;

    public void RenderDialog(DialogItem dialogItem)
    {
        if(dialogItem.Id == -1)
        {
            Destroy(dialogPanel);
            return;
        }
        dialogPanel.transform.GetChild(2).GetChild(0).gameObject.GetComponent<UnityEngine.UI.Text>().text = dialogItem.Question;
        foreach (Transform child in dialogPanel.transform.GetChild(2).GetChild(1))
        {
            GameObject.Destroy(child.gameObject);
        }
        int index = 0;
        foreach (var answer in dialogItem.Answers)
        {
            GameObject Answer = Instantiate(AnswerPrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            Answer.transform.SetParent(GameObject.Find("AnswerArea").transform, false);
            Answer.transform.localPosition = new Vector3(0, (dialogItem.Answers.Length / 2) * (-40) + (index * 40), 0);
            Answer.transform.GetChild(0).gameObject.GetComponent<UnityEngine.UI.Text>().text = answer.InAnswerText;
            Answer.GetComponent<Button>().onClick.AddListener(delegate { RenderDialog(answer); });
            index = index + 1;
        }
    }

    public void TestDialogFunction()
    {
        dialogPanel = Instantiate(DialogWindowPrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        dialogPanel.transform.SetParent(GameObject.Find("Canvas").transform, false);
        RenderDialog(TestDialog);
    }
}
