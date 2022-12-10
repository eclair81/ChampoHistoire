using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //[SerializeField] private List<Dialogue> allDialogues;
    [SerializeField] private List<EventObject> allObjects;
    private int index;

    [SerializeField] private ObjectEventDialogue objectEventDialoguePrefab;
    private ObjectEventDialogue currentObjectEvent;

    [SerializeField] private GameObject dialogueUI;
    private DialogueManager dialogueManager;

    public static GameManager Instance;
    public GameState currentGameState;

    private void Awake()
    {
        Instance = this;

        index = 0;
        dialogueManager = dialogueUI.GetComponentInChildren<DialogueManager>();

        currentGameState = GameState.Grid;
    }

    public IEnumerator SpawnObject(EventObject eventObject, Vector2 pos)
    {
        currentGameState = GameState.Dialogue;

        //spawn object
        currentObjectEvent = Instantiate(objectEventDialoguePrefab, pos, Quaternion.identity);
        StartCoroutine(currentObjectEvent.Spawn(eventObject));

        //wait spawn animation
        yield return new WaitForSeconds(1.5f);

        //start dialogue
        ShowDialogueUI(eventObject.dialogue);
    }

    public void ShowDialogueUI(Dialogue newDialogue)
    {
        dialogueUI.SetActive(true);
        dialogueManager.StartDialogue(newDialogue);
    }

    public void HideDialogueUI()
    {
        currentGameState = GameState.Grid;
        dialogueUI.SetActive(false);
        StartCoroutine(currentObjectEvent.PutAway(new Vector2(-1.5f, -3)));
    }

    //Call this function from an Event Tile
    public EventObject SendThisTileEventObject()
    {
        //Dialogue thisDialogue = allDialogues[index];
        EventObject thisEventObject = allObjects[index];
        index++; //Increment for next time

        return thisEventObject;
    }
}

[System.Serializable]
public class EventObject
{
    public Dialogue dialogue;
    public Sprite sprite;
}

public enum GameState
{
    Grid,
    Dialogue
}