using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectEventDialogue : MonoBehaviour
{
    public Dialogue thisEventDialogue;
    //public AnimationCurve animationCurve;

    private SpriteRenderer spriteRenderer;
    private Vector3 scaleBase;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        scaleBase = transform.localScale;
    }

    public IEnumerator Spawn(EventObject eventObject)
    {
        spriteRenderer.sprite = eventObject.sprite;
        //Debug.Log("spawned!");

        for(int i=0; i < 100; i++)
        {
            transform.localScale = scaleBase * i;
            transform.localRotation = Quaternion.Euler(new Vector3(0, 3.6f * i, 0));
            yield return new WaitForSeconds(0.015f);
        }
    }

    public IEnumerator PutAway(Vector2 newpos)
    {

        for (int i = 100; i > 20; i--)
        {
            transform.localScale = scaleBase * i;
            transform.position = Vector2.Lerp(transform.position, newpos, 0.05f);
            yield return new WaitForSeconds(0.015f);
        }
    }
}
