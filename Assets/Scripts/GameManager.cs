using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI levelDisplay;
    public SuccessDisplay successDisplay;
    public static GameManager Instance;
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
    public bool IsComplete(GridSystem.GridElementLevel level)
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
    public void DisplayCurrentLevel()
    {
        if (levelDisplay == null) return;
        levelDisplay.text = "Level " + SerializeJson.lastLoadedLevel.ToString();
    }

    public void DisplaySuccess()
    {
        successDisplay.DisplaySuccess();
    }

}
