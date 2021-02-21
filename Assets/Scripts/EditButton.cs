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
    public EditButtonState state = EditButtonState.Floor;
    private static EditButton currentStart;

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
    public void ResetToFloor()
    {
        state = EditButtonState.Floor;
        StopAllCoroutines();
        colorRoutine = StartCoroutine(ColorCell(Color.white));
    }

    public void MouseClick()
    {
        if (state == EditButtonState.Invisible) return;

        Color color = Color.white;
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
        {
            if (currentStart != null)
                currentStart.ResetToFloor();

            state = EditButtonState.Start;
            currentStart = this;
            color = Color.red;
        }
        else if (state == EditButtonState.Floor)
        {
            state = EditButtonState.Wall;
            color = Color.cyan;
        }
        else
        {
            state = EditButtonState.Floor;
            color = Color.white;
        }
        StopAllCoroutines();
        colorRoutine = StartCoroutine(ColorCell(color));
    }
}
