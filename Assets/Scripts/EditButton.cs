using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Wrj;

public enum EditButtonState { Floor, Wall, Start, Invisible }
// Buttons representing puzzle units in the EditorView
public class EditButton : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
{
    // Associated UI
    public Image button;
    // current state reference
    private EditButtonState _state = EditButtonState.Floor;

    // static edit button reference to maintain only a single starting point
    private static EditButton _currentStart;
    // static bool that returns whether there's a starting unit set in the edit.
    // This is referenced by the test button, to avoid starting game mode without
    // a start position for the player ball.
    public static bool IsCurrentStartSet { get { return _currentStart != null; } }
        
    // Whenever the state is changed, also manipulate the color
    public EditButtonState State
    {
        get { return _state; }
        set
        {
            if (value != _state)
            {
                Color color = Color.white;
                switch (value)
                {
                    case EditButtonState.Floor:
                        color = UIColor.Clouds;
                        break;
                    case EditButtonState.Wall:
                        color = UIColor.PeterRiver;
                        break;
                    case EditButtonState.Start:
                        {
                            // Make the previous starting unit a floor, allowing only one start position.
                            if (_currentStart != null)
                                _currentStart.State = EditButtonState.Floor;
                            // Set this button as the start
                            _currentStart = this;
                            color = UIColor.Pomegranate;
                            break;
                        }
                    case EditButtonState.Invisible:
                        color = UIColor.Concrete;
                        break;
                    default:
                        break;
                }
                // discontinue other color changes
                StopAllCoroutines();
                // change the cell to the appropriate color
                StartCoroutine(ColorCell(color));
                // remember this state
                _state = value;
            }
        }
    }

    // Change the color of the cell over time
    IEnumerator ColorCell(Color color)
    {
        // get the current color so we know what to interpolate from
        Color startColor = button.color;
        float elapsedTime = 0f;
        while(elapsedTime < .25f)
        {
            yield return new WaitForEndOfFrame();
            // add the time elapsed since the last frame to the timer
            elapsedTime += Time.deltaTime;
            // set the color to the color between start and target color, proportional to the elapsed time
            button.color = Color.Lerp(startColor, color, Mathf.InverseLerp(0f, .25f, elapsedTime));
        }
        // finally, set directly to the desired target color
        button.color = color;
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        // treat entering the cell with the mouse button down the same as a click
        if (Input.GetMouseButton(0))
        {
            MouseClick();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        MouseClick();
    }

    // Handle the toggling behavior of the buttons
    public void MouseClick()
    {
        // Don't allow manipulation of invisible cells
        if (State == EditButtonState.Invisible) return;

        Color color = Color.white;
        // if the CTRL key is down, make this button the start pos
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
        {
            State = EditButtonState.Start;
        }
        // if we're a floor, become a wall (and vice versa)
        else if (State == EditButtonState.Floor)
        {
            State = EditButtonState.Wall;
        }
        else
        {
            State = EditButtonState.Floor;
        }
    }
}
