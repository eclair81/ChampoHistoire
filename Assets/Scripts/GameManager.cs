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

    private bool newObject;

    private void Awake()
    {
        Instance = this;

        index = 0;
        dialogueManager = dialogueUI.GetComponentInChildren<DialogueManager>();

        currentGameState = GameState.Grid;

        newObject = false;
    }

    public IEnumerator SpawnObject(EventObject eventObject, Vector2 pos)
    {
        currentGameState = GameState.Dialogue;

        //spawn object
        currentObjectEvent = Instantiate(objectEventDialoguePrefab, pos, Quaternion.identity);
        StartCoroutine(currentObjectEvent.Spawn(eventObject));
        newObject = true;

        //wait spawn animation
        yield return new WaitForSeconds(1.5f);

        //start dialogue
        ShowDialogueUI(eventObject.initialDialogue);
    }

    public void ShowDialogueUI(Dialogue newDialogue)
    {
        currentGameState = GameState.Dialogue; // Needs to be repeated because ShowDialogueUI can be called from the object when it's in the inventory

        dialogueUI.SetActive(true);
        dialogueManager.StartDialogue(newDialogue);
    }

    public void HideDialogueUI()
    {
        currentGameState = GameState.Grid;
        dialogueUI.SetActive(false);

        //Checks if This function is called after a new object was found (to avoid repeating the "put away" animation if it's an object from the inventory)
        if (newObject)
        {
            newObject = false;
            StartCoroutine(currentObjectEvent.PutAway(new Vector2(-1.5f, -3)));
        }
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
    public Dialogue initialDialogue;
    public Dialogue inventoryDialogue;
    public Sprite sprite;
}

public enum GameState
{
    Grid,
    Dialogue
}