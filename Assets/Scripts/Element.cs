using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Element : MonoBehaviour
{
    public int index;
    public bool isWall = false;
    private bool isWallCached = false;
    public bool isInvisible = false;
    private bool isInvisibleCached = false;
    public bool isStartPosition = false;
    private bool isStartPositionCached = false;
    private bool isCollected = false;


    private void Update()
    {
        if (isWall != isWallCached || isInvisible != isInvisibleCached || isStartPosition != isStartPositionCached)
        {
            isWallCached = isWall;
            isInvisibleCached = isInvisible;
            isStartPositionCached = isStartPosition;
            GridSystem.Instance.ResetGrid();
        }
    }

    public void SetCollected()
    {
        isCollected = true;
        transform.Color(Color.blue, .25f);
    }
    public bool GetCollected()
    {
        return isCollected;
    }
}
