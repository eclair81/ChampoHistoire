using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnim : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();   
    }

    //cf Convert AngleToDir
    //Rotate the gameobject to face to correct direction
    public void RotateToDir(int dir)
    {
        transform.localRotation = Quaternion.Euler(new Vector3(0, -90 * dir, 0));
    }

    public void PlayAnimation(string triggerName)
    {
        animator.SetTrigger(triggerName);
    }

    public void StopAnimation()
    {
        animator.SetTrigger("Stop");
    }
}
