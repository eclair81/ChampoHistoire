using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchInput : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if(Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.touches[0].position), Vector2.zero);

            if(hit.collider != null)
            {
                //Check if it's an adjacent tile
                if(MoveOnGrid.Instance.TryToMove(new Vector2(hit.transform.position.x, hit.transform.position.y)))
                {
                    //Debug.Log ("Target Position: " + hit.collider.gameObject.transform.position);
                    hit.collider.GetComponent<Tile>().Interact();
                }
            }
        }
    }
}
