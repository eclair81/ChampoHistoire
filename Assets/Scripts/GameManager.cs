using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<Dialogue> allDialogues;
    private int dialogueIndex;
    [SerializeField] private GameObject dialogueUI;
    private DialogueManager dialogueManager;

    public static GameManager Instance;
    public GameState currentGameState;

    private void Awake()
    {
        Instance = this;

        dialogueIndex = 0;
        dialogueManager = dialogueUI.GetComponentInChildren<DialogueManager>();

        currentGameState = GameState.Grid;
    }


    public void ShowDialogueUI(Dialogue newDialogue)
    {
        currentGameState = GameState.Dialogue;
        dialogueUI.SetActive(true);
        dialogueManager.StartDialogue(newDialogue);
    }

    public void HideDialogueUI()
    {
        currentGameState = GameState.Grid;
        dialogueUI.SetActive(false);
    }

    //Call this function from an Event Tile
    public Dialogue SendThisTileDialogue()
    {
        Dialogue thisDialogue = allDialogues[dialogueIndex];
        dialogueIndex++; //Increment for next time

        return thisDialogue;
    }
}

public enum GameState
{
    Grid,
    Dialogue
}