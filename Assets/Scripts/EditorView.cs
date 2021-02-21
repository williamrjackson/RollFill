using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class EditorView : MonoBehaviour
{
    public EditButton buttonPrototype = null;
    public Image panel = null;
    public int dimensionsX = 10;
    public int dimensionsY = 10;
    int cachedDimensionsX = 0;
    int cachedDimensionsY = 0;

    public List<EditButton> buttonList = new List<EditButton>();

    // Start is called before the first frame update
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

    // Update is called once per frame
    void Update()
    {
        if (buttonPrototype != null) 
        {
            if (dimensionsX != cachedDimensionsX || 
                dimensionsY != cachedDimensionsY)
            {
                ClearEditButtons();
                GridLayoutGroup grid = GetComponent<GridLayoutGroup>();
                grid.cellSize = Vector2.one * (640 / (dimensionsX + 2));

                for (int i = 0; i < dimensionsX + 2; i++)
                {
                    for (int j = 0; j < dimensionsY + 2; j++)
                    {
                        EditButton newButton = Instantiate(buttonPrototype);
                        newButton.transform.SetParent(transform);
                        newButton.gameObject.SetActive(true);
                        buttonList.Add(newButton);
                        if (i == 0 || i == dimensionsX + 1 || j == 0 || j == dimensionsY + 1)
                        {
                            newButton.state = EditButtonState.Invisible;
                            newButton.transform.Color(Color.gray, .1f);
                        }
                    }
                }
                cachedDimensionsX = dimensionsX;
                cachedDimensionsY = dimensionsY;
                grid.constraintCount = dimensionsY + 2;
            }
        }
    }
}
