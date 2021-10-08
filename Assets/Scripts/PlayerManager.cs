using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    [HideInInspector]public bool goodEnding;
    public int scrollsToOpen;

    private PlayerMovement movementScript;
    private CameraView cameraScript;

    private static PlayerManager instance;
   
    public static PlayerManager Instance { private set; get; }

    private int collectedScrolls = 0;

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

    public int GetCollectedScrolls() => collectedScrolls;

    public void AddCollectedScroll() => collectedScrolls += 1;
    
}
