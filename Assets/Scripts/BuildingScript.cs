using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingScript : MonoBehaviour
{
    [SerializeField] private Color notTransparent;
    [SerializeField] private Color transparent;

    private SpriteRenderer spriteRenderer;
    private Vector3 displacement; // used to render the building on top off the player when the player is "inside"

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        displacement = new Vector3(0, 0, -2f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //ignore triggers during the switching level animation -> to avoid some batiments being missplaced during minimazing/maximazing 
        if (GameManager.Instance.currentGameState == GameState.Transition) return;

        spriteRenderer.color = transparent;
        transform.position += displacement;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        spriteRenderer.color = notTransparent;
        transform.position -= displacement;
    }
}
