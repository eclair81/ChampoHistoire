using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes; //https://dbrizov.github.io/na-docs/

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

    [HideIf("whoIsTalking", CurrentlyTalking.Narrator)]
    [AllowNesting]
    public Expression whichExpression;

    [HideIf("whoIsTalking", CurrentlyTalking.Narrator)]
    [AllowNesting]
    public Animation whichAnimation;

    public TypeOfText thisTextIs;

    [ShowIf("thisTextIs", TypeOfText.JumpAfter)]
    [AllowNesting]
    public string jumpTo;

    [ShowIf("thisTextIs", TypeOfText.Question)]
    [AllowNesting]
    public Answer repA;

    [ShowIf("thisTextIs", TypeOfText.Question)]
    [AllowNesting]
    public Answer repB;
}

[System.Serializable]
public class Answer
{
    [BoxGroup("Answers")]
    public string answer;
    [BoxGroup("Answers")]
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

public enum Animation
{
    UpDown,
    Tantrum
}

public enum TypeOfText
{
    Normal,
    JumpAfter,
    Question
}