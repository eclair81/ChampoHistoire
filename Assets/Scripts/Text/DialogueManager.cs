using UnityEngine;
using System.Collections;
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

    [SerializeField] private DialogueCharacter professorChar;
    [SerializeField] private DialogueCharacter otherChar;
    [Header("Sprites for all characters")]
    [SerializeField] private CharactersSprites allCharactersSprites;



    private MyText currentText;
    private int currentTextIndex;

    // Start is called before the first frame update
    void Start()
    {
        //get next false button ref
        nextFalseButton = nextButton.transform.GetChild(0).gameObject;

        //get choice buttons text ref
        choice1 = choiceContainer.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>(); // get button -> get TMP
        choice2 = choiceContainer.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();

        //set professor here
        professorChar.UpdateCharacterInfo(CurrentlyTalking.Professor, allCharactersSprites.professor);
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

    public void StartDialogue(Dialogue newDialogue)
    {
        dialogue = newDialogue;

        currentTextIndex = 0;
        currentText = dialogue.listeText[currentTextIndex];
        ResetVariables();
        SetOther(dialogue.firstCharNotProf); // set correct other character from the start

        //check if prof starts alone
        if (currentText.isAloneAtStart)
        {
            otherChar.gameObject.SetActive(false);
        }
        else
        {
            otherChar.gameObject.SetActive(true);
        }

        UpdateWhosTalking();
    }

    private void AddLetter()
    {
        displayText.maxVisibleCharacters ++;

        if (displayText.maxVisibleCharacters >= currentText.text.Length)
        {
            printingText = false;
            //Show button since text is now fully printed
            if (currentText.thisTextIs == TypeOfText.Question)
            {
                //show choice container
                DeactivateNextButton(); // To prevent going to next dialogue without answering the question 
                ShowChoiceContainer();
            }
            else
            {
                ShowFalseNextButton();
            }

            //stop talking animation
            professorChar.StopAnim();
            otherChar.StopAnim();
        }
    }

    //Wrapper for the coroutine since it can't be called directly from the button
    public void NextTextButton(int value)
    {
        StartCoroutine(NextText(value));
    }

    public IEnumerator NextText(int value)
    {
        if(value == 0) // next button clicked
        {
            //Check if text is already fully printed or if it's a touch to quickly advance
            if (printingText)
            {
                //display full text
                displayText.maxVisibleCharacters = currentText.text.Length;
                yield break;
            }

            if (currentText.thisTextIs == TypeOfText.JumpAfter) //jump in the dialogue
            {
                currentTextIndex = GetTextIndex(currentText.jumpTo);
            }
            else
            {
                currentTextIndex++;
                //If at end of Dialogue list -> Hide DialogueUI
                if (dialogue.listeText.Count == currentTextIndex) 
                {
                    GameManager.Instance.HideDialogueUI();
                    yield break;
                }
            }
        }
        else // clicked on either choice1 or choice2 button
        {
            switch (value)
            {
                case 1:
                    currentTextIndex = GetTextIndex(currentText.repA.jumpTo);
                    break;
                case 2:
                    currentTextIndex = GetTextIndex(currentText.repB.jumpTo);
                    break;
            }
        }

        if (currentText.leavesTheStage)
        {
            StartCoroutine(otherChar.LeaveStage());
            yield return new WaitUntil(() => otherChar.ok);
            otherChar.gameObject.SetActive(false);
        }


        //next Dialogue box
        currentText = dialogue.listeText[currentTextIndex];
        if (currentText.entersTheStage)
        {
            otherChar.gameObject.SetActive(true);
            SetOther(currentText.whoIsTalking);
            StartCoroutine(otherChar.EnterStage());
            yield return new WaitUntil(() => otherChar.ok);
        }

        ResetVariables();
        UpdateWhosTalking();

        //Prepare Answers Buttons
        if (currentText.thisTextIs == TypeOfText.Question)
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
        choice1.text = currentText.repA.answer;
        choice2.text = currentText.repB.answer;
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
        professorChar.UpdateCharacter(dialogue.listeText[currentTextIndex].whoIsTalking, dialogue.listeText[currentTextIndex].whichExpression, dialogue.listeText[currentTextIndex].whichAnimation);
        otherChar.UpdateCharacter(dialogue.listeText[currentTextIndex].whoIsTalking, dialogue.listeText[currentTextIndex].whichExpression, dialogue.listeText[currentTextIndex].whichAnimation);
    }

    private void SetOther(CurrentlyTalking setToWho)
    {
        SpritesExpression correctExpression;

        switch (setToWho)
        {
            case CurrentlyTalking.Mme_Gilbert:
                correctExpression = allCharactersSprites.mme_Gilbert;
                break;
            case CurrentlyTalking.Soldat_1:
                correctExpression = allCharactersSprites.soldat_1;
                break;
            case CurrentlyTalking.Soldat_2:
                correctExpression = allCharactersSprites.soldat_2;
                break;
            case CurrentlyTalking.Soldat_3:
                correctExpression = allCharactersSprites.soldat_3;
                break;
            case CurrentlyTalking.Soldat_4:
                correctExpression = allCharactersSprites.soldat_4;
                break;
            case CurrentlyTalking.Etudiant_1:
                correctExpression = allCharactersSprites.etudiant_1;
                break;
            case CurrentlyTalking.Etudiant_2:
                correctExpression = allCharactersSprites.etudiant_2;
                break;
            //Not supposed to pass by default case.
            default:
                correctExpression = allCharactersSprites.professor;
                break;
        }

        otherChar.UpdateCharacterInfo(setToWho, correctExpression);
    }
}

[System.Serializable]
public class CharactersSprites
{
    public SpritesExpression professor;
    public SpritesExpression mme_Gilbert;
    public SpritesExpression soldat_1;
    public SpritesExpression soldat_2;
    public SpritesExpression soldat_3;
    public SpritesExpression soldat_4;
    public SpritesExpression etudiant_1;
    public SpritesExpression etudiant_2;
}

[System.Serializable]
public class SpritesExpression
{
    public Sprite neutral;
    public Sprite happy;
    public Sprite sad;
    public Sprite angry;
    public Sprite fear;
}