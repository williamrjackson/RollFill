using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

[ExecuteInEditMode]
public class EditorView : MonoBehaviour
{
    public EditButton buttonPrototype = null;
    public int dimensionsX = 10;
    public int dimensionsY = 10;
    int cachedDimensionsX = 0;
    int cachedDimensionsY = 0;

    List<EditButton> buttonList = new List<EditButton>();

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
        if (buttonPrototype == null) return;

        if (dimensionsX != cachedDimensionsX || 
            dimensionsY != cachedDimensionsY)
        {
            ClearEditButtons();
            for (int i = 0; i < dimensionsX; i++)
            {
                for (int j = 0; j < dimensionsY; j++)
                {
                    EditButton newButton = Instantiate(buttonPrototype);
                    newButton.transform.SetParent(transform);
                    newButton.gameObject.SetActive(true);
                    buttonList.Add(newButton);
                }
            }
            cachedDimensionsX = dimensionsX;
            cachedDimensionsY = dimensionsY;
            GetComponent<GridLayoutGroup>().constraintCount = dimensionsY;
        }
    }
}
