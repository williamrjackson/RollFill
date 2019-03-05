﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
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
        Debug.Log("Moving from " + currentPosition);
        List<Element> elements = GridSystem.Instance.GetUnitsFromPosition(currentPosition, dir);
        float dur = elements.Count * .05f;
        
        if (elements.Count > 1)
        {
            isMoving = true;
            Wrj.Utils.MapToCurve.EaseIn.Move(transform, elements[elements.Count - 1].transform.localPosition.With(y:transform.localPosition.y), dur, onDone: EndMove);
            currentPosition = elements[elements.Count - 1].index;
            float delay = 0f;
            foreach (Element item in elements)
            {
                Wrj.Utils.Delay(delay, () => item.SetCollected());
                delay += .05f;
            }
        }
    }
    void EndMove()
    {
        isMoving = false;
    }
}