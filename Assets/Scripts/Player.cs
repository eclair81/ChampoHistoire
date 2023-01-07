using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Vector3 targetPos;

    [SerializeField] private CharacterAnim playerAnimator;
    [SerializeField] private CharacterAnim profAnimator;

    private GameObject hitbox1;
    private GameObject hitbox2;
    private GameObject profGameObject;

    [HideInInspector] public int lastDirFromInput;
    [HideInInspector] public bool isWalking;
    private bool notAlone;

    private void Start()
    {
        hitbox1 = transform.GetChild(0).gameObject;
        hitbox2 = transform.GetChild(1).gameObject;
        profGameObject = transform.GetChild(3).gameObject;

        //Player is alone on first level
        if (GameManager.Instance.levelIndex != 0)
        {
            notAlone = true;

            UpdateHitbox();
            profGameObject.SetActive(true);
        }
    }

    public void UpdatePos()
    {
        Vector2 newTargetPos = Convert.GridToIso(MoveOnGrid.Instance.GetPos());

        targetPos = newTargetPos;
        StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        isWalking = true;

        //play walking animation
        playerAnimator.RotateToDir(lastDirFromInput);
        playerAnimator.PlayAnimation("WalkAnim");
        if (notAlone)
        {
            profAnimator.RotateToDir(lastDirFromInput);
            profAnimator.PlayAnimation("WalkAnim");
        }
        

        Vector3 oldPos = transform.position;
        float dist = Vector3.Distance(oldPos, targetPos);
        for (float i = 0.0f; i < 1.0f; i += Time.deltaTime / dist)
        {
            transform.position = Vector3.Lerp(oldPos, targetPos, i);
            yield return new WaitForSeconds(0.01f);
        }

        isWalking = false;
        playerAnimator.StopAnimation();
        if(notAlone) profAnimator.StopAnimation();
    }

    //Call this function from GameManager after the new level finishes it's animation to avoid the player being displayed on top of a building 
    public void UpdateHitbox()
    {
        //deactive the hitboxes first
        hitbox1.SetActive(false);
        hitbox2.SetActive(false);
 
        if (notAlone)
        {
            hitbox2.SetActive(true);
        }
        else
        {
            hitbox1.SetActive(true);
        }
    } 
}