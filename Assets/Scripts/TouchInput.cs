using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchInput : MonoBehaviour
{
    private Vector2 point1;
    private Vector2 point2;
    
    // Update is called once per frame
    void Update()
    {
        if(Input.touchCount > 0) 
        {
            if(Input.touches[0].phase == TouchPhase.Began)
            {
                point1 = Camera.main.ScreenToWorldPoint(Input.touches[0].position);
            }
            
            if(Input.touches[0].phase == TouchPhase.Ended)
            {
                point2 = Camera.main.ScreenToWorldPoint(Input.touches[0].position);
                CalculateSwipeDirection(point1, point2);
            }
        }
    }

    private void CalculateSwipeDirection(Vector2 p1, Vector2 p2)
    {
        Vector2 vectDir = p2 - p1;
        vectDir.Normalize();

        // if true, didn't swipe -> directly touched a tile
        if(vectDir == new Vector2(0, 0))
        {
            SendRaycast(Camera.main.ScreenToWorldPoint(Input.touches[0].position));
            return;
        }

        // Get the swipe direction
        float sign = (vectDir.y >= 0) ? 1 : -1; // get if the swipe is upward or downward
        float offset = (sign == 1) ? 0 : 360;   // add an offset to get a 0-360° angle
        float angle = Vector2.Angle(new Vector2(1, 0), vectDir) * sign + offset;
        AngleToDir(angle);
    }

    private void AngleToDir(float angle)
    {
        Vector2 currentPos = MoveOnGrid.Instance.GetPos();

        if(angle <= 90f)    //(angle <= 90f || angle >= 315f)
        {
            //Debug.Log("Up-Right");    //Debug.Log("Right");
            SendRaycast(Convert.GridToIso(new Vector2(currentPos.x + 1, currentPos.y)));
            return;
        }
        if(angle > 90f && angle <= 180f)    //(angle > 45f && angle <= 135f)
        {
            //Debug.Log("Up-Left");    //Debug.Log("Up");
            SendRaycast(Convert.GridToIso(new Vector2(currentPos.x, currentPos.y + 1)));
            return;
        }
        if(angle > 180f && angle <= 270f)   //(angle > 135f && angle <= 225f)
        {
            //Debug.Log("Down-Left");   //Debug.Log("Left");
            SendRaycast(Convert.GridToIso(new Vector2(currentPos.x - 1, currentPos.y)));
            return;
        }
        if(angle > 270f)    //(angle > 225f && angle < 315f)
        {
            //Debug.Log("Down-Right");   //Debug.Log("Down");
            SendRaycast(Convert.GridToIso(new Vector2(currentPos.x, currentPos.y - 1)));
            return;
        }
    }

    private void SendRaycast(Vector2 touchPos)
    {
        RaycastHit2D hit = Physics2D.Raycast(touchPos, Camera.main.transform.forward);

        if(hit.collider != null)
        {
            //Check if it's an adjacent tile
            if(MoveOnGrid.Instance.TryToMove(new Vector2(hit.transform.position.x, hit.transform.position.y)))
            {
                //Debug.Log ("Target Position: " + hit.collider.gameObject.transform.position);
                hit.collider.GetComponent<Tile>().Interact();
                //Debug.Log("grid: " + Convert.IsoToGrid(hit.collider.gameObject.transform.position));
            }
        }
    }
}
