using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Range(.01f, .1f)]
    public float durationPerUnit = .025f;
    public TouchAxisCtrl touchAxisCtrl;
    public static Player Instance;

    private int currentPosition;
    private bool isMoving = false;

    void Awake ()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    void Start()
    {
        touchAxisCtrl.OnSwipe += this.OnSwipe;
    }

    private void OnSwipe(TouchAxisCtrl.Direction direction)
    {
        switch (direction)
        {
            case TouchAxisCtrl.Direction.Up:
                Move(GridSystem.Direction.Up);
                break;
            case TouchAxisCtrl.Direction.Down:
                Move(GridSystem.Direction.Down);
                break;
            case TouchAxisCtrl.Direction.Left:
                Move(GridSystem.Direction.Left);
                break;
            case TouchAxisCtrl.Direction.Right:
                Move(GridSystem.Direction.Right);
                break;
            default:
                break;
        }
    }

    public void SetCurrentPos(int currentPos)
    {
        currentPosition = currentPos;
    }

    void Update()
    {
        if (isMoving) return;

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Move(GridSystem.Direction.Left);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Move(GridSystem.Direction.Right);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Move(GridSystem.Direction.Up);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Move(GridSystem.Direction.Down);
        }
    }

    void Move(GridSystem.Direction dir)
    {
        // Get the list of elements that can be traversed in the chosen direction
        List<Element> elements = GridSystem.Instance.GetUnitsFromPosition(currentPosition, dir);
        // set the travel duration relative to the number of elements to traverse
        float dur = elements.Count * durationPerUnit;
        
        if (elements.Count > 1)
        {
            isMoving = true;
            // move the ball. Call "EndMove()" when the movement is completed to see if this move wins the level
            Wrj.Utils.MapToCurve.EaseIn.Move(transform, elements[elements.Count - 1].transform.localPosition.With(y:transform.localPosition.y), dur, onDone: EndMove);
            // set the current position to te last element, the destination 
            currentPosition = elements[elements.Count - 1].index;

            // "Collect" the traversed elements.
            // do it progressively by adding more of a delay
            // to each collection.
            float delay = 0f;
            foreach (Element item in elements)
            {
                Wrj.Utils.DeferredExecution(delay, () => item.SetCollected());
                delay += durationPerUnit;
            }
        }
    }

    // Check if the level's complete after each move.
    void EndMove()
    {
        GridSystem.Instance.CheckForWin();
        isMoving = false;
    }
}
