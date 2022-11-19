using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private Dialogue dialogue; 
    [SerializeField] private TextMeshProUGUI displayText;
    [SerializeField] private float delayBetweenLetters = 0.1f;
    private float betweenLettersTimer;
    private bool printingText;
    private int currentLetter;

    [SerializeField] private GameObject nextButton;
    [SerializeField] private GameObject choiceContainer;
    private TextMeshProUGUI choice1;
    private TextMeshProUGUI choice2;

    /*private int currentNumberOfChar; // current number of characters printed from the current message
    private int maxNumberOfCharPerBox = 100;
    */


    private MyText currentText;
    private string textToPrint;

    private int currentTextID;

    // Start is called before the first frame update
    void Start()
    {
        printingText = true;
        betweenLettersTimer = 0f;
        currentLetter = 0;
        currentTextID = 0;
        textToPrint = "";
        currentText = dialogue.listeText[currentTextID];

        //get choice buttons text ref
        choice1 = choiceContainer.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>(); // get button -> get TMP
        choice2 = choiceContainer.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if(printingText)
        {
            betweenLettersTimer += Time.deltaTime;
            if(betweenLettersTimer >= delayBetweenLetters)
            {
                betweenLettersTimer = 0f;
                AddLetter();
            }
        }
    }

    private void AddLetter()
    {
        char letter = currentText.text[currentLetter];
        textToPrint += letter;
        displayText.text = textToPrint;

        currentLetter ++;
        if(currentLetter == currentText.text.Length) printingText = false; 
    }

    public void NextText(int value)
    {
        Debug.Log("next!");

        //reset variables
        displayText.text = "";
        textToPrint = "";
        printingText = true;
        betweenLettersTimer = 0f;
        currentLetter = 0;

        if(value == 0) // next button clicked
        {
            //increment TextID
            currentTextID++;
        }
        else // clicked on either choice1 or choice2 button
        {
            currentTextID += currentText.answerList[value - 1].advanceDialogueBy;
        }
        currentText = dialogue.listeText[currentTextID];

        //Display Choice Container or Next Button
        if(currentText.isQuestion)
        {
            showChoiceContainer();
            FillChoiceButtons();
        }
        else
        {
            showNextButton();
        }
    }

    private void FillChoiceButtons()
    {
        choice1.text = currentText.answerList[0].answer;
        choice2.text = currentText.answerList[1].answer;
    }

    private void showNextButton()
    {
        nextButton.SetActive(true);
        choiceContainer.SetActive(false);
    }

    private void showChoiceContainer()
    {
        nextButton.SetActive(false);
        choiceContainer.SetActive(true);
    }
}
