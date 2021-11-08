using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Mert Karavural
//Date: 11 Oct 2020


[CreateAssetMenu(fileName = "DialogItem", menuName = "Inventuna/Heroes Chained/Dialog Item", order = 1)]
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
