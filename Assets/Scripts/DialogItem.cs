using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "DialogItem", menuName = "Inventuna/Heroes_Chained/DialogItem", order = 1)]
public class DialogItem : ScriptableObject
{
    public int Id;
    public string Question;
    public string InAnswerText;
    public int Reserved1;
    public string Reserved2;
    public Object Reserved3;
    public DialogItem[] Answers;

}
