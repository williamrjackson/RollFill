using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class EditorView : MonoBehaviour
{
    public EditButton buttonPrototype = null;
    public int dimensionsX = 10;
    public int dimensionsY = 10;
    public List<EditButton> buttonList = new List<EditButton>();

    // The inspector can be edited and not match. These are managed by the Build method.
    [HideInInspector]
    public int effectiveDimensionsX = 0;
    [HideInInspector]
    public int effectiveDimensionsY = 0;

    public void Reset()
    {
        Build(true);
    }
    
    void Start()
    {
        LoadLevel();
//        Wrj.Utils.DeferredExecution(2f, () => LoadLevel());
    }

    void ClearEditButtons()
    {
        if (buttonList.Count > 0)
        {
            foreach (EditButton item in buttonList)
            {
                DestroyImmediate(item.gameObject);
            }
            buttonList.Clear();
        }
    }

    public void LoadLevel()
    {
        var level = SerializeJson.EditLevel;
        if (level == null)
        {
            Reset();
            return;
        }
        dimensionsX = level.rows;
        dimensionsY = level.columns;
        Build();
        Debug.Log("Count: " + level.unitCubes.Length);
        for (int i = 0; i < level.unitCubes.Length; i++)
        {
            UnitCube element = level.unitCubes[i]; 
            if (element.isWall)
            {
                buttonList[i].State = EditButtonState.Wall;
            }
            else
            {
                buttonList[i].State = EditButtonState.Floor;
            }
            if (element.isInvisible)
            {
                buttonList[i].State = EditButtonState.Invisible;
            }
        }
        buttonList[level.startPosition].State = EditButtonState.Start;
    }

    void Build(bool fresh = false)
    {
        ClearEditButtons();
        GridLayoutGroup grid = GetComponent<GridLayoutGroup>();
        grid.cellSize = Vector2.one * (640 / (dimensionsX));

        for (int i = 0; i < dimensionsX; i++)
        {
            for (int j = 0; j < dimensionsY; j++)
            {
                EditButton newButton = Instantiate(buttonPrototype);
                newButton.transform.SetParent(transform);
                newButton.gameObject.SetActive(true);
                buttonList.Add(newButton);
                if (fresh)
                {
                    if (i == 0 || i == dimensionsX - 1 || j == 0 || j == dimensionsY - 1)
                    {
                        newButton.State = EditButtonState.Wall;
                    }
                }
            }
        }
        effectiveDimensionsX = dimensionsX;
        effectiveDimensionsY = dimensionsY;
        grid.constraintCount = effectiveDimensionsY;
    }
}
