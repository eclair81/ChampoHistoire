using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private List<CoupleSprite> spritesVariation;
    private SpriteRenderer spriteRenderer;
    private int randomVariationIndex;

    private Vector3 posBase;
    private bool alreadyInteracted;
    private string myState;

    [SerializeField] private AnimationCurve curve;
    private float lastFrameCurve;
    private float animTimer = 0f;
    private bool doAnim = false;

    //public Dialogue thisEventDialogue;
    public EventObject thisEventObject;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        alreadyInteracted = false;
        posBase = transform.position;

        //Select which sprites to use among the variations
        randomVariationIndex = (int)Random.Range(0f, (float)spritesVariation.Count - 0.001f); // -0.001 because Random.Range is inclusive

        //Get length of the animation curve
        Keyframe lastFrame = curve[curve.length - 1];
        lastFrameCurve = lastFrame.time;
    }

    void Update()
    {
        if(doAnim)
        {
            Anim();
        }
    }

    public void Init(string etat)
    {
        spriteRenderer.sprite = spritesVariation[randomVariationIndex].baseSprite;
        myState = etat;

        //force sprite order base on y value (low y -> high order)
        //spriteRenderer.sortingOrder = 100 - (int)(transform.position.y * 4); // *4 because each row is 0.25 higher than the last -> 0.25 * 4 -> sprite order decreases by 1 each row

        switch (myState)
        {
            case "start":
                Interact();
                break;
            case "event":
                thisEventObject = GameManager.Instance.SendThisTileEventObject();
                break;
        }
    }

    public void Interact()
    {
        //Only interact once with each tile
        if(alreadyInteracted) return;
        alreadyInteracted = true;

        spriteRenderer.sprite = spritesVariation[randomVariationIndex].interacted;

        if (myState == "event")
        {
            StartCoroutine(GameManager.Instance.SpawnObject(thisEventObject, transform.position));
        }

        if (myState == "prof")
        {
            Debug.Log("on prof tile");
        }

        // Disable tile anim to avoid animating the tile without moving the decor on top of it
        // (an easy fix would be to instantiate the decor elements with the tile as a parent, but project manager said animating the tile wasn't in the GDD)
        //doAnim = true; 
    }

    private void Anim()
    {
        animTimer += Time.deltaTime;
        if(animTimer <= lastFrameCurve)
        {
            transform.position = posBase + new Vector3(0, curve.Evaluate(animTimer));
            return;
        }
        transform.position = posBase;
        doAnim = false;
    }
}

[System.Serializable]
public class CoupleSprite
{
    public Sprite baseSprite;
    public Sprite interacted;
}