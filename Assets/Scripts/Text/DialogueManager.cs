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

    [SerializeField] private GameObject nextButton;
    private GameObject nextFalseButton;
    [SerializeField] private GameObject choiceContainer;
    private TextMeshProUGUI choice1;
    private TextMeshProUGUI choice2;

    private MyText currentText;
    private int currentTextID;

    // Start is called before the first frame update
    void Start()
    {
        currentTextID = 0;
        currentText = dialogue.listeText[currentTextID];
        ResetVariables();

        //get next false button ref
        nextFalseButton = nextButton.transform.GetChild(0).gameObject;

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
        displayText.maxVisibleCharacters ++;

        if (displayText.maxVisibleCharacters >= currentText.text.Length)
        {
            printingText = false;
            //Show button since text is now fully printed
            if (currentText.isQuestion)
            {
                //show choice container
                DeactivateNextButton(); // To prevent going to next dialogue without answering the question 
                ShowChoiceContainer();
            }
            else
            {
                //show false next button
                ShowNextButton();
            }
        }
    }

    public void NextText(int value)
    {
        if(value == 0) // next button clicked
        {
            //Check if text is already fully printed or if it's a touch to quickly advance
            if (printingText)
            {
                //display full text
                displayText.maxVisibleCharacters = currentText.text.Length;
                return;
            }

            if (currentText.needToSkipAfterThisText)
            {
                currentTextID += currentText.advanceDialogueBy; //skip some dialogue
            }
            else
            {
                currentTextID++;
            }
        }
        else // clicked on either choice1 or choice2 button
        {
            currentTextID += currentText.answerList[value - 1].advanceDialogueBy;
        }
        currentText = dialogue.listeText[currentTextID];
        ResetVariables();

        //Display Choice Container or Next Button
        if (currentText.isQuestion)
        {
            ActivateNextButton(); // To allow to instantly display the question
            HideNextButton();     // Hide false next button from previous text
            FillChoiceButtons();
        }
        else
        {
            HideChoiceContainer();
            ActivateNextButton();
            HideNextButton(); // Hide false next button from previous text
        }
    }

    private void ResetVariables() 
    {
        printingText = true;
        betweenLettersTimer = 0f;
        displayText.maxVisibleCharacters = 1;
        displayText.text = currentText.text;
    }

    private void FillChoiceButtons()
    {
        choice1.text = currentText.answerList[0].answer;
        choice2.text = currentText.answerList[1].answer;
    }

    private void ActivateNextButton()
    {
        nextButton.SetActive(true);
    }

    private void DeactivateNextButton() 
    {
        nextButton.SetActive(false);
    }

    private void ShowNextButton() 
    {
        nextFalseButton.SetActive(true);
    }

    private void HideNextButton()
    {
        nextFalseButton.SetActive(false);
    }

    private void ShowChoiceContainer()
    {
        choiceContainer.SetActive(true);
    }

    private void HideChoiceContainer() 
    {
        choiceContainer.SetActive(false);
    }
}
