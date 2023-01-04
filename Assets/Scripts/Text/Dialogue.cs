using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes; //https://dbrizov.github.io/na-docs/

[CreateAssetMenu(fileName = "Dialogue", menuName = "Data/Dialogue", order = 0)]
public class Dialogue : ScriptableObject 
{
    public CurrentlyTalking firstCharNotProf;
    public List<MyText> listeText;
}

[System.Serializable]
public class MyText
{
    public string text;
    public string id;
    public CurrentlyTalking whoIsTalking;

    [ShowIf("whoIsTalking", CurrentlyTalking.Professor)]
    [AllowNesting]
    public bool isAloneAtStart;

    [HideIf("whoIsTalking", CurrentlyTalking.Professor)]
    [AllowNesting]
    public bool entersTheStage;

    [HideIf("whoIsTalking", CurrentlyTalking.Professor)]
    [AllowNesting]
    public bool leavesTheStage;

    public Expression whichExpression;

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
    Professor,
    Mme_Gilbert,
    Soldat_1,
    Soldat_2,
    Soldat_3,
    Soldat_4,
    Etudiant_1,
    Etudiant_2
}

public enum Expression
{
    Neutral,
    Happy,
    Sad, 
    Angry,
    Fear
}

public enum Animation
{
    UpDown,
    Tantrum,
    NoAnim
}

public enum TypeOfText
{
    Normal,
    JumpAfter,
    Question
}