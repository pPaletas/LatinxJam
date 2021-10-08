using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    #region Singleton
    public static CheckpointManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    #endregion

    [SerializeField]private List<Transform> checkPoints = new List<Transform>();

    private int currentCheckpoint = 0;

    public void SetTransformAsCheckpoint(Transform checkpoint)
    {
        currentCheckpoint = checkPoints.LastIndexOf(checkpoint);
    }

    public void RespawnPlayer()
    {
        Vector3 checkPointPos = checkPoints[currentCheckpoint].position;
        PlayerManager.Instance.SetPosition(checkPointPos);
        PlayerManager.Instance.LookAt(checkPointPos + (checkPoints[currentCheckpoint].forward * 5f));
    }
}