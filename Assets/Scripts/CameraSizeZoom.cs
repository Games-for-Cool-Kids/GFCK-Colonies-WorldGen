using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSizeZoom : MonoBehaviour
{
    public float factor = 0.1f;

    void Update()
    {
        GetComponent<Camera>().orthographicSize += Input.mouseScrollDelta.y * -factor;
    }
}
