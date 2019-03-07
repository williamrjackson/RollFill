using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static public bool IsComplete(GridSystem.GridElementLevel level)
    {
        foreach (Element element in level.elements)
        {
            if (!element.isWall && !element.GetCollected())
            {
                return false;
            }
        }
        return true;
    }
}
