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
            camera.orthographicSize = Mathf.Clamp(camera.orthographicSize, 1, 20);
        }
    }

    private void Move()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            if (Input.GetMouseButton(0))
            {
                MoveCamera();
            }
        }
        else if (Input.GetMouseButton(2))
        {
            MoveCamera();
        }
        
    }

    private void MoveCamera()
    {
#if UNITY_EDITOR
        transform.position += new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), 0) * (-1 * camera.orthographicSize * Time.deltaTime);
#else
                transform.position += new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), 0) * (-1 * camera.orthographicSize * Time.deltaTime * 10);
#endif
                
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -5, 20), Mathf.Clamp(transform.position.y, -120, 120), transform.position.z);
    }
}
