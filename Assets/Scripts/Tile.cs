using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Color color1, color2, color3, color4;
    private SpriteRenderer spriteRenderer;
    private bool alreadyInteracted;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        alreadyInteracted = false;
    }

    public void Init(bool isOffset)
    {
        spriteRenderer.color = isOffset ? color1 : color2;
    }

    public void Interact()
    {
        if(alreadyInteracted) return;

        spriteRenderer.color = (spriteRenderer.color == color1) ? color3 : color4;
        alreadyInteracted = true;
    }
}
