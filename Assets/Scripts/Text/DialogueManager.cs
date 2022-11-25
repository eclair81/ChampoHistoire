using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private Dialogue dialogue; 
    [SerializeField] private TextMeshProUGUI displayText;
    [SerializeField] private float delayBetweenLetters = 0.1f;
    private float betweenLettersTimer;
    private bool printingText;

    [SerializeField] private GameObject nextButton; // real next button -> can be clicked on
    private GameObject nextFalseButton;             // false next button -> just a visual gimmick
    [SerializeField] private GameObject choiceContainer;
    private TextMeshProUGUI choice1;
    private TextMeshProUGUI choice2;

    [SerializeField] private DialogueCharacter playerChar;
    [SerializeField] private DialogueCharacter professorChar;

    private MyText currentText;
    private int currentTextIndex;

    // Start is called before the first frame update
    void Start()
    {
        currentTextIndex = 0;
        currentText = dialogue.listeText[currentTextIndex];
        ResetVariables();
        UpdateWhosTalking();

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
                ShowFalseNextButton();
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

            if (currentText.jumpTo != "") //jump in the dialogue
            {
                currentTextIndex = GetTextIndex(currentText.jumpTo);
            }
            else
            {
                currentTextIndex++;
            }
        }
        else // clicked on either choice1 or choice2 button
        {
            currentTextIndex = GetTextIndex(currentText.answerList[value - 1].jumpTo);
        }
        currentText = dialogue.listeText[currentTextIndex];
        ResetVariables();
        UpdateWhosTalking();

        //Display Choice Container or Next Button
        if (currentText.isQuestion)
        {
            ActivateNextButton();  // To allow to instantly display the question
            HideFalseNextButton(); // Hide false next button from previous text
            FillChoiceButtons();
        }
        else
        {
            HideChoiceContainer();
            ActivateNextButton();
            HideFalseNextButton(); // Hide false next button from previous text
        }
    }

    private void ResetVariables() 
    {
        printingText = true;
        betweenLettersTimer = 0f;
        displayText.maxVisibleCharacters = 1;
        displayText.text = currentText.text;
    }

    private int GetTextIndex(string targetTextId)
    {
        for(int i=0; i < dialogue.listeText.Count; i++)
        {
            if (dialogue.listeText[i].id == targetTextId) return i;
        }
        //target id not found
        return currentTextIndex++;
    }

    private void FillChoiceButtons()
    {
        choice1.text = currentText.answerList[0].answer;
        choice2.text = currentText.answerList[1].answer;
    }

    #region button functions
    private void ActivateNextButton()
    {
        nextButton.SetActive(true);
    }

    private void DeactivateNextButton() 
    {
        nextButton.SetActive(false);
    }

    private void ShowFalseNextButton() 
    {
        nextFalseButton.SetActive(true);
    }

    private void HideFalseNextButton()
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
    #endregion

    //Grey out the non talking character, Change Sprite according to expression
    private void UpdateWhosTalking()
    {
        playerChar.UpdateCharacter(dialogue.listeText[currentTextIndex].whoIsTalking, dialogue.listeText[currentTextIndex].whichExpression);
        professorChar.UpdateCharacter(dialogue.listeText[currentTextIndex].whoIsTalking, dialogue.listeText[currentTextIndex].whichExpression);
    }
}
