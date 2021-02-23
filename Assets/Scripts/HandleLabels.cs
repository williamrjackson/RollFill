using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HandleLabels : MonoBehaviour
{
    public TMP_InputField inputX;
    public TMP_InputField inputY;
    public Button resetButton;
    public EditorView editorView;

    public void UpdateValues()
    {
        Color current = Color.white;
        Color edited = Color.yellow;
        bool xEdited = false;
        bool yEdited = false;
        if (int.TryParse(inputX.text, out int xVal))
        {
            if (editorView.xDimensions != xVal)
            {
                editorView.xDimensions = xVal;
                xEdited = (editorView.xDimensions != editorView.effectiveXDimensions);
            }
        }
        if (int.TryParse(inputY.text, out int yVal))
        {
            if (editorView.yDimensions != yVal) 
            {
                editorView.yDimensions = yVal;
                yEdited = (editorView.yDimensions != editorView.effectiveYDimensions);
            }       
        }

        if (xEdited || yEdited)
        {
            resetButton.image.transform.Color(edited, 1f);
            if (xEdited)
            {
                inputX.image.transform.Color(edited, 1f);
            }
            else
            {
                inputX.image.transform.Color(current, 1f);
            }
            if (yEdited)
            {
                inputY.image.transform.Color(edited, 1f);
            }
            else
            {
                inputY.image.transform.Color(current, 1f);
            }
        }
        else
        {
            resetButton.image.transform.Color(current, 1f);
            inputY.image.transform.Color(current, 1f);
            inputX.image.transform.Color(current, 1f);
        }
    }

    public void UpdateFields()
    {
        inputX.text = editorView.xDimensions.ToString();
        inputY.text = editorView.yDimensions.ToString();
        UpdateValues();
    }
}
