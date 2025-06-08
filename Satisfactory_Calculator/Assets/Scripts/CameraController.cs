using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera camera;

    private void Update()
    {
        Zoom();
        Move();
    }

    private void Zoom()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            camera.orthographicSize -= Input.GetAxis("Mouse ScrollWheel") * 10;
            camera.orthographicSize = Mathf.Clamp(camera.orthographicSize, 1, 10);
        }
    }

    private void Move()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            if (Input.GetMouseButton(0))
            {
                transform.position += new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), 0) * (-0.01f * camera.orthographicSize);
                transform.position = new Vector3(Mathf.Clamp(transform.position.x, -5, 20), Mathf.Clamp(transform.position.y, -50, 50), transform.position.z);
            }
        }
    }
}
