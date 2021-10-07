using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [HideInInspector]public bool goodEnding;

    private PlayerMovement movementScript;
    private CameraView cameraScript;

    public static PlayerManager Instance { private set; get; }

    private void Awake()
    {
        Instance = this;
        movementScript = GetComponent<PlayerMovement>();
        cameraScript = GetComponentInChildren<CameraView>();
    }

    public void EnableControls()
    {
        movementScript.EnablePlayerMovement();
        cameraScript.EnableCameraControl();
    }

    public void DisableControls()
    {
        movementScript.DisablePlayerMovement();
        cameraScript.DisableCameraControl();
    }

    public void LookAt(Vector3 targetPosition)
    {
        movementScript.LookAt(targetPosition);
        cameraScript.transform.LookAt(targetPosition);
    }
}
