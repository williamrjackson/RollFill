using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuccessDisplay : MonoBehaviour
{
    [SerializeField]
    private string[] successTexts;
    [SerializeField]
    private TMPro.TextMeshProUGUI textMeshProObject;

    private void Awake()
    {
        if (textMeshProObject == null)
        {
            textMeshProObject = GetComponent<TMPro.TextMeshProUGUI>();
        }
    }
    
    public void DisplaySuccess()
    {
        textMeshProObject.text = GetRandomSuccessString();
        gameObject.EaseScale(Vector3.one, 1.5f);
        Wrj.Utils.Delay(2f, () => gameObject.Scale(Vector3.zero, .5f));
    }

    private string GetRandomSuccessString()
    {
        return successTexts[Random.Range(0, successTexts.Length)];
    }
}
