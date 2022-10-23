using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Color color1, color2, color3, color4;
    private SpriteRenderer spriteRenderer;
    private bool alreadyInteracted;
    private bool isEventTile; 

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        alreadyInteracted = false;
        isEventTile = false;
    }

    public void Init(bool isOffset, bool isEvent)
    {
        spriteRenderer.color = isOffset ? color1 : color2;
        isEventTile = isEvent;
    }

    public void Interact()
    {
        //Only interact once with each tile
        if(alreadyInteracted) return;
        alreadyInteracted = true;

        spriteRenderer.color = (spriteRenderer.color == color1) ? color3 : color4;

        if(isEventTile)
        {
            Debug.Log("You interacted with an event tile!");
        }
        
    }
}
