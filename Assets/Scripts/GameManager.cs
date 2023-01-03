using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

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

    [SerializeField] private GameObject nextLevelUI;
    private GameObject sliderGameObject;
    private Slider slider;
    private TextMeshProUGUI year1;
    private TextMeshProUGUI year2;
    private TextMeshProUGUI year3;
    public bool sliderTouched;

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

        //Get sliderGameObject and years references 
        sliderGameObject = nextLevelUI.transform.GetChild(1).gameObject;
        slider = sliderGameObject.GetComponent<Slider>();
        year1 = sliderGameObject.transform.GetChild(3).GetComponentInChildren<TextMeshProUGUI>();
        year2 = sliderGameObject.transform.GetChild(4).GetComponentInChildren<TextMeshProUGUI>();
        year3 = sliderGameObject.transform.GetChild(5).GetComponentInChildren<TextMeshProUGUI>();

        currentGameState = GameState.Grid;

        newObject = false;
        sliderTouched = false;
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
        currentLevel.GetComponentInChildren<GridGenerator>().GenerateCustomGrid(levelList[levelIndex].level, levelList[levelIndex].tileUsed, levelList[levelIndex].decor.allDecorOnThisLevel, levelList[levelIndex].buildings.buildingsOnThisLevel);
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
        UpdateSliderYears();
        nextLevelUI.SetActive(false);
        MoveOnGrid.Instance.UpdateLevelIndex(levelIndex);

        SpawnLevel();
    }

    public IEnumerator SpawnObject(EventObject eventObject, Vector2 pos)
    {
        currentGameState = GameState.Dialogue;

        //Get currentLevel Inventory container
        Transform inventory = allLevelsContainer.GetChild(levelIndex).GetChild(2);

        //spawn object
        Vector3 correctedPos = new Vector3(pos.x, pos.y, -3f); // added Z axis to avoid the object sprite being renderer under the player mesh
        currentObjectEvent = Instantiate(objectEventDialoguePrefab, correctedPos, Quaternion.identity, inventory);
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
            StartCoroutine(currentObjectEvent.PutAway(new Vector3(newPosX, newPosY, -3f)));

            levelList[levelIndex].objectFound++;
            if (levelList[levelIndex].objectFound == levelList[levelIndex].objectInLevel.Count)
            {
                Debug.Log("Found all objects on this level !");
                //Maybe add a dialogue here to explain slider for the first time
                nextLevelUI.SetActive(true); //Show next level button & slider
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

    public void UpdateSliderYears()
    {
        year1.text = levelList[levelIndex].sliderInfo.year1.ToString();
        year2.text = levelList[levelIndex].sliderInfo.year2.ToString();
        year3.text = levelList[levelIndex].sliderInfo.year3.ToString();
    }

    public void IsCorrectYearSelected()
    {
        int sliderValue = (int)slider.value;
        if (sliderValue == levelList[levelIndex].sliderInfo.correctSliderPos)
        {
            //Maybe start a conversation before loading the next level?
            //Add a transition effect?
            NextLevel();
            return;
        }

        Debug.Log("Bad year selected");
        //Maybe start a conversation explaining why it's not the right choice ?
    }

    //only useful to avoid moving the player when touching the slider
    public void ChangedSliderValue()
    {
        sliderTouched = true;
    }
}


[System.Serializable]
public class InfoLevel
{
    public Texture2D level;
    public GameObject tileUsed;
    public Decor decor;
    public Buildings buildings;
    [HideInInspector]public int objectFound;
    public List<EventObject> objectInLevel;
    public SliderInfo sliderInfo;
}

[System.Serializable]
public class EventObject
{
    public Dialogue initialDialogue;
    public Dialogue inventoryDialogue;
    public Sprite sprite;
}

[System.Serializable]
public class SliderInfo
{
    [Range(0, 3)]
    public int correctSliderPos;
    public int year1;
    public int year2;
    public int year3;
}

public enum GameState
{
    Grid,
    Dialogue
}