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
        if(displayText.maxVisibleCharacters >= currentText.text.Length) printingText = false; 
    }

    public void NextText(int value)
    {
        if(value == 0) // next button clicked
        {
            //increment TextID
            currentTextID++;
            //TODO, add a "advanceDialogueBy" option to regular text
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
            showChoiceContainer();
            FillChoiceButtons();
        }
        else
        {
            showNextButton();
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
