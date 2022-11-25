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
    public string id;
    public CurrentlyTalking whoIsTalking;
    public Expression whichExpression;
    public string jumpTo;
    public bool isQuestion;
    public List<Answer> answerList;
}

[System.Serializable]
public class Answer
{
    public string answer;
    public string jumpTo;
}

public enum CurrentlyTalking
{
    Narrator,
    Player,
    Professor
}

public enum Expression
{
    Happy,
    Sad, 
    Angry,
    Fear
}