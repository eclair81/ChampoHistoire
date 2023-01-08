using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchInput : MonoBehaviour
{
    private Vector2 point1;
    private Vector2 point2;

    [SerializeField] private Player player;
    
    public bool useMouseInput; //mouse input for when i forget my usb cable

    // Update is called once per frame
    void Update()
    {
        if(useMouseInput)
        {
            if(Input.GetMouseButtonDown(0))
            {
                //Debug.Log("down");
                point1 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
            if(Input.GetMouseButtonUp(0))
            {
                //Debug.Log("Up");
                point2 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                CalculateSwipeDirection(point1, point2);
            }
        }
        else
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
    }

    private void CalculateSwipeDirection(Vector2 p1, Vector2 p2)
    {
        Vector2 vectDir = p2 - p1;
        vectDir.Normalize();

        int dir;

        // if true, didn't swipe -> directly touched a tile
        if(vectDir == new Vector2(0, 0))
        {
            if(useMouseInput)
            {
                //Calculate direction
                dir = Convert.AngleToDir((Vector2)Input.mousePosition - (Vector2)player.transform.position);
                player.lastDirFromInput = dir;

                SendRaycast(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                return;
            }

            //Calculate direction
            dir = Convert.AngleToDir((Vector2)Input.touches[0].position - (Vector2)player.transform.position);
            player.lastDirFromInput = dir;

            SendRaycast(Camera.main.ScreenToWorldPoint(Input.touches[0].position));
            return;
        }

        // Get the swipe direction
        dir = Convert.AngleToDir(vectDir);
        player.lastDirFromInput = dir;
        Vector2 currentPos = MoveOnGrid.Instance.GetPos();

        switch(dir)
        {
            case 0:
                SendRaycast(Convert.GridToIso(new Vector2(currentPos.x + 1, currentPos.y)));
                break;
            case 1:
                SendRaycast(Convert.GridToIso(new Vector2(currentPos.x, currentPos.y + 1)));
                break;
            case 2:
                SendRaycast(Convert.GridToIso(new Vector2(currentPos.x - 1, currentPos.y)));
                break;
            case 3:
                SendRaycast(Convert.GridToIso(new Vector2(currentPos.x, currentPos.y - 1)));
                break;
        }
    }

    private void SendRaycast(Vector2 touchPos)
    {
        //Disable movement when not in Grid state (ex: Dialogue state)
        if (GameManager.Instance.currentGameState != GameState.Grid) return;

        //Don't move if already walking
        if (player.isWalking) return;

        //Don't move if interacting with the year slider
        if (GameManager.Instance.sliderTouched)
        {
            GameManager.Instance.sliderTouched = false;
            return;
        }

        RaycastHit2D hit = Physics2D.Raycast(touchPos, Camera.main.transform.forward);

        if(hit.collider != null)
        {
            if (hit.collider.GetComponent<ObjectEventDialogue>())
            {
                hit.collider.GetComponent<ObjectEventDialogue>().Interact();
                return;
            }

            //Check if it's an adjacent tile
            if(MoveOnGrid.Instance.TryToMove(new Vector2(hit.transform.position.x, hit.transform.position.y)))
            {
                //Debug.Log ("Target Position: " + hit.collider.gameObject.transform.position);
                hit.collider.GetComponent<Tile>().Interact();
                //Debug.Log("grid: " + Convert.IsoToGrid(hit.collider.gameObject.transform.position));
                player.UpdatePos();
            }
        }
    }
}
