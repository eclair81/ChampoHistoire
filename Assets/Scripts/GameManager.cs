using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<InfoLevel> levelList;
    public int levelIndex;
    private int objectIndex;
    private bool newObject;

    [Header("Levels")]
    [SerializeField] private GameObject levelPrefab;
    [SerializeField] private Transform allLevelsContainer;
    [Header("Dialogue")]
    [SerializeField] private ObjectEventDialogue objectEventDialoguePrefab;
    private ObjectEventDialogue currentObjectEvent;

    [SerializeField] private GameObject dialogueUI;
    private DialogueManager dialogueManager;

    [SerializeField] private GameObject nextLevelButton;

    public static GameManager Instance;
    [HideInInspector] public GameState currentGameState;

    //Inventory position
    private float[,] inv;

    private void Awake()
    {
        Instance = this;

        inv = new float[,] { { -1.5f, -3f }, { 0f, -3f }, { 1.5f, -3f }};

        objectIndex = 0;
        levelIndex = -1;

        dialogueManager = dialogueUI.GetComponentInChildren<DialogueManager>();

        currentGameState = GameState.Grid;

        newObject = false;
    }

    private void Start()
    {
        MoveOnGrid.Instance.AddSpaceInAllPos(levelList.Count);
        NextLevel();
    }

    //Spawn new level according to levelIndex
    public void SpawnLevel()
    {
        GameObject currentLevel = Instantiate(levelPrefab, allLevelsContainer);
        currentLevel.GetComponentInChildren<GridGenerator>().GenerateCustomGrid(levelList[levelIndex].level, levelList[levelIndex].tileUsed);
    }

    public void NextLevel()
    {
        //Hide previous level
        if (levelIndex != -1)
        {
            allLevelsContainer.GetChild(levelIndex).gameObject.SetActive(false);
        }

        levelIndex++;

        if (levelIndex == levelList.Count)
        {
            Debug.Log("The End");
            return;
        }

        objectIndex = 0;
        nextLevelButton.SetActive(false);
        MoveOnGrid.Instance.UpdateLevelIndex(levelIndex);

        SpawnLevel();
    }

    public IEnumerator SpawnObject(EventObject eventObject, Vector2 pos)
    {
        currentGameState = GameState.Dialogue;

        //Get currentLevel Inventory container
        Transform inventory = allLevelsContainer.GetChild(levelIndex).GetChild(2);

        //spawn object
        currentObjectEvent = Instantiate(objectEventDialoguePrefab, pos, Quaternion.identity, inventory);
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

            float newPosX = inv[levelList[levelIndex].objectFound, 0];
            float newPosY = inv[levelList[levelIndex].objectFound, 1];
            StartCoroutine(currentObjectEvent.PutAway(new Vector2(newPosX, newPosY)));

            levelList[levelIndex].objectFound++;
            if (levelList[levelIndex].objectFound == levelList[levelIndex].objectInLevel.Count)
            {
                Debug.Log("Found all objects on this level !");
                nextLevelButton.SetActive(true); //Show next level button for now (change later)
            }
        }
    }

    //Call this function from an Event Tile
    public EventObject SendThisTileEventObject()
    {
        EventObject thisEventObject = levelList[levelIndex].objectInLevel[objectIndex];
        objectIndex++; //Increment for next time

        return thisEventObject;
    }

    public int NumberOfObjectsInLevel()
    {
        return levelList[levelIndex].objectInLevel.Count;
    }
}


[System.Serializable]
public class InfoLevel
{
    public Texture2D level;
    public GameObject tileUsed;
    [HideInInspector]public int objectFound;
    public List<EventObject> objectInLevel;
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