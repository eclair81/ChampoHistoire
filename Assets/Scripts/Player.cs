using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float animDelay;
    private float animTimer = 0f;

    private SpriteRenderer spriteRenderer;
    [SerializeField] private List<Sprite> spriteSheet0 = new List<Sprite>();
    [SerializeField] private List<Sprite> spriteSheet1 = new List<Sprite>();
    [SerializeField] private List<Sprite> spriteSheet2 = new List<Sprite>();
    [SerializeField] private List<Sprite> spriteSheet3 = new List<Sprite>();

    private List<Sprite[]> spriteSheet = new List<Sprite[]>();

    private int sheetNumber = 0;
    private int spriteNumber = 0;

    private Vector3 targetPos;
    [SerializeField]private float posTransDelay = 1;
    private float posTransTimer = 0f;
    private bool doSmoothTransition = false;
    [SerializeField] private float lerpTime = 0.1f;

    /*void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        spriteSheet.Add(spriteSheet0.ToArray());
        spriteSheet.Add(spriteSheet1.ToArray());
        spriteSheet.Add(spriteSheet3.ToArray());
        spriteSheet.Add(spriteSheet2.ToArray());
    }*/

    void Update()
    {
        //SpriteAnim();

        if(doSmoothTransition)
        {
            SmoothTransition();
        }
    }

    public void UpdatePos()
    {
        Vector2 newTargetPos = Convert.GridToIso(MoveOnGrid.Instance.GetPos());

        //sheetNumber = Convert.AngleToDir(newTargetPos - (Vector2)transform.position); // Update sheetNumber to current moving direction
        //spriteRenderer.sprite = spriteSheet[sheetNumber][spriteNumber]; // Quick update to display correct direction before next normal changement

        //transform.position = targetPos;
        targetPos = newTargetPos;
        posTransTimer = 0f;
        doSmoothTransition = true;
    }

    private void SmoothTransition()
    {
        transform.position = Vector3.Lerp(transform.position, targetPos, lerpTime);

        posTransTimer += Time.deltaTime;
        if(posTransTimer >= posTransDelay)
        {
            posTransTimer = 0f;
            doSmoothTransition = false;
            transform.position = targetPos;
        }
    }

    /*
    private void SpriteAnim()
    {
        animTimer += Time.deltaTime;
        if(animTimer >= animDelay)
        {
            animTimer = 0f;
            spriteNumber = (spriteNumber + 1) % spriteSheet[sheetNumber].Length; // Next sprite in sheet, loop
            spriteRenderer.sprite = spriteSheet[sheetNumber][spriteNumber];
        }
    }
    */
}
