using UnityEngine;

public class ViewportCamera : MonoBehaviour
{
    public float speed = 0.1f;

    void Update()
    {
        var camera = GetComponent<Camera>();

        // Zoom
        camera.orthographicSize += Input.mouseScrollDelta.y * -speed;

        // Move
        var pos = transform.position;
        pos += Vector3.right * Input.GetAxis("Horizontal") * speed;
        pos += Vector3.up * Input.GetAxis("Vertical") * speed;
        transform.position = pos;
    }
}
