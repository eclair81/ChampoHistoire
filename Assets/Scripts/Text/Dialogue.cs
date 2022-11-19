using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue", menuName = "Data/Dialogue", order = 0)]
public class Dialogue : ScriptableObject 
{
    public List<MyText> listeText;
}

[System.Serializable]
public class MyText
{
    public string text;
    public bool isQuestion;
    public List<Answer> answerList;
}

[System.Serializable]
public class Answer
{
    public string answer;
    public int advanceDialogueBy;
}