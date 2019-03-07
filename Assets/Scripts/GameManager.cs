using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI levelDisplay;
    public TextMeshProUGUI successDisplay;
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
        levelDisplay.text = "Level " + SaveFile.lastLoadedLevel.ToString();
    }
    public void DisplaySuccess()
    {
        if (successDisplay == null) return;
        successDisplay.gameObject.EaseScale(Vector3.one, 1.5f);
        Wrj.Utils.Delay(2f, () => successDisplay.gameObject.Scale(Vector3.zero, .5f));
    }
}
