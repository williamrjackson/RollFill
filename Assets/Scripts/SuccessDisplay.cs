using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuccessDisplay : MonoBehaviour
{
    [Header("Timing")]
    [SerializeField]
    [Range(0f, 3f)]
    private float delay = 0f;
    [SerializeField]
    [Range(0f, 3f)]
    private float inTime = 1.5f;
    [SerializeField]
    [Range(0f, 3f)]
    private float hangTime = 2f;
    [SerializeField]
    [Range(0f, 3f)]
    private float outTime = .5f;
    [Header("Text Options")]
    [SerializeField]
    private string[] successTexts = null;
    [SerializeField]
    private TMPro.TextMeshProUGUI textMeshProObject;
    [SerializeField]
    private Wrj.Utils.MapToCurve scaleCurve = null;

    private void Awake()
    {
        if (textMeshProObject == null)
        {
            textMeshProObject = GetComponent<TMPro.TextMeshProUGUI>();
        }
        gameObject.Scale(Vector3.zero, 0f);
    }
    
    public void DisplaySuccess()
    {
        Wrj.Utils.DeferredExecution(delay, () => Display());
    }
    private void Display()
    {
        textMeshProObject.text = GetRandomSuccessString();
        scaleCurve.Scale(transform, Vector3.one, inTime);
        Wrj.Utils.DeferredExecution(hangTime, () => gameObject.Scale(Vector3.zero, outTime));
    }

    private string GetRandomSuccessString()
    {
        return successTexts[Random.Range(0, successTexts.Length)];
    }
}
