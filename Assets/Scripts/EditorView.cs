using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class EditorView : MonoBehaviour
{
    public EditButton buttonPrototype = null;
    public int xDimensions = 10;
    public int yDimensions = 10;
    public List<EditButton> buttonList = new List<EditButton>();

    // The inspector can be edited and not match. These are managed by the Build method.
    [HideInInspector]
    public int effectiveXDimensions = 0;
    [HideInInspector]
    public int effectiveYDimensions = 0;

    // Called by the Reset button.
    // Makes a new grid based on dimensions
    // in the inspector.
    public void Reset()
    {
        // true "fresh" flag to produce border walls
        Build(true);
    }
    
    void Start()
    {
        // load the edit level if possible on start
        LoadLevel();
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

    // Read in level0.json and modify button states accordingly
    public void LoadLevel()
    {
        var level = SerializeJson.EditLevel;
        if (level == null)
        {
            Reset();
            return;
        }
        yDimensions = level.rows;
        xDimensions = level.columns;
        // Make the right number of buttons
        Build();

        // Set each button state
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
            // Invisible is also floor, so do this explicitely
            if (element.isInvisible)
            {
                buttonList[i].State = EditButtonState.Invisible;
            }
        }
        // Set the start button by start position index
        buttonList[level.startPosition].State = EditButtonState.Start;
    }

    // Build a grid of buttons. "Fresh" builds auto-set a border of walls.
    void Build(bool fresh = false)
    {
        // Remove any existing buttons
        ClearEditButtons();
        
        // Set the grid size
        GridLayoutGroup grid = GetComponent<GridLayoutGroup>();
        grid.cellSize = Vector2.one * (640 / (yDimensions));

        // Add buttons based on the disabled prototype in the hierarchy
        for (int i = 0; i < yDimensions; i++)
        {
            for (int j = 0; j < xDimensions; j++)
            {
                EditButton newButton = Instantiate(buttonPrototype);
                newButton.transform.SetParent(transform);
                newButton.gameObject.SetActive(true);
                buttonList.Add(newButton);
                if (fresh)
                {
                    if (i == 0 || i == yDimensions - 1 || j == 0 || j == xDimensions - 1)
                    {
                        newButton.State = EditButtonState.Wall;
                    }
                }
            }
        }
        // set "effective" dimensions, so external references don't get values
        // that have been updated in the inspector, but never applied to the 
        // actual grid.
        effectiveYDimensions = yDimensions;
        effectiveXDimensions = xDimensions;

        // Set the grid constraint to the number of desired columns
        grid.constraintCount = effectiveXDimensions;
    }
}
