using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraView : MonoBehaviour
{

    public float mouseSensitivity;

    private float mouseX;
    private float mouseY;
    private bool enabledCamera = true;

    public Transform target;

    public Transform playerBody;
    private float rotationX = 0f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void DisableCameraControl()
    {
        enabled = false;
    }

    public void EnableCameraControl()
    {
        enabled = true;
    }

    void Update()
    {

        if(enabledCamera)
        {
            mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            rotationX -= mouseY;
            rotationX = Mathf.Clamp(rotationX, -90f, 90f);

            transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
            playerBody.Rotate(Vector3.up * mouseX);
        }

    }
    
}
