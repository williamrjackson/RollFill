using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMan : MonoBehaviour
{
    public Vector3 min = new Vector3(0f, 5f, -3.5f);
    public Vector3 max= new Vector3(0f, 14.5f, -10f);
    [Range(0f, 3f)]
    public float changeDuration = 1.5f;
    public static CameraMan Instance;
    void Awake ()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void MoveCameraToFit(int widthHeight)
    {
        Mathf.Clamp(widthHeight, 5, 16);
        Vector3 targetPos = Vector3.Lerp(min, max, (float)widthHeight.Remap(5f, 16f, 0f, 1f));
        Wrj.Utils.MapToCurve.Ease.Move(transform, targetPos, changeDuration);
    }

}
