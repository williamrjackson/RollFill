using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum EditButtonState { Floor, Wall, Start, Invisible }
public class EditButton : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
{
    public Image button;
    private Coroutine colorRoutine;
    private EditButtonState _state = EditButtonState.Floor;
    private static EditButton _currentStart;
    public static bool IsCurrentStartSet { get { return _currentStart != null; } }
        

    public EditButtonState State
    {
        get { return _state; }
        set
        {
            if (value != _state)
            {
                Debug.Log(Enum.GetName(typeof(EditButtonState), _state));
                Color color = Color.white;
                switch (value)
                {
                    case EditButtonState.Floor:
                        color = Color.white;
                        break;
                    case EditButtonState.Wall:
                        color = Color.cyan;
                        break;
                    case EditButtonState.Start:
                        {
                            if (_currentStart != null)
                                _currentStart.State = EditButtonState.Floor;
                            _currentStart = this;
                            color = Color.red;
                            break;
                        }
                    case EditButtonState.Invisible:
                        color = Color.gray;
                        break;
                    default:
                        break;
                }
                StopAllCoroutines();
                colorRoutine = StartCoroutine(ColorCell(color));
                _state = value;
            }
        }
    }

    IEnumerator ColorCell(Color color)
    {
        Color startColor = button.color;
        float elapsedTime = 0f;
        while(elapsedTime < .25f)
        {
            yield return new WaitForEndOfFrame();
            elapsedTime += Time.deltaTime;
            button.color = Color.Lerp(startColor, color, Mathf.InverseLerp(0f, .25f, elapsedTime));
        }
        button.color = color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Input.GetMouseButton(0))
        {
            MouseClick();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        MouseClick();
    }

    public void MouseClick()
    {
        if (State == EditButtonState.Invisible) return;

        Color color = Color.white;
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
        {
            State = EditButtonState.Start;
        }
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
