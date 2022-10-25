using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Color color1, color2, color3, color4;
    private SpriteRenderer spriteRenderer;
    private bool alreadyInteracted;
    private string myState; 

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        alreadyInteracted = false;
    }

    public void Init(bool isOffset, string etat)
    {
        spriteRenderer.color = isOffset ? color1 : color2;
        myState = etat;

        if(myState == "start") Interact();
    }

    public void Interact()
    {
        //Only interact once with each tile
        if(alreadyInteracted) return;
        alreadyInteracted = true;

        spriteRenderer.color = (spriteRenderer.color == color1) ? color3 : color4;

        if(myState == "event")
        {
            Debug.Log("You interacted with an event tile!");
        }
        
    }
}
