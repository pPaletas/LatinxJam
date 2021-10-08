using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyMovement : MonoBehaviour
{
    [HideInInspector] public Transform movePositionTransform;
    [HideInInspector] public NavMeshAgent navMeshAgent;

    public bool canMove = false;
    [Header("Player Interaction")]
    [SerializeField]private float distanceToKillPlayer;
    [SerializeField]private FadePanel fadePanel;
    [SerializeField]private float fadeTime; 

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        fadePanel.OnBlackScreen.AddListener(OnScreenBlacked);
        fadePanel.OnComplete.AddListener(OnFadeFinish);
    }

    private void Update()
    {
        if (canMove)
        {
            if (navMeshAgent.isStopped) navMeshAgent.isStopped = false;
            navMeshAgent.destination = movePositionTransform.position;

            KillPlayerOnReached();
        }
        else if (!navMeshAgent.isStopped)
        {
            navMeshAgent.isStopped = true;
        }
    }

    void KillPlayerOnReached()
    {
        Vector3 targetDistance = (movePositionTransform.position - transform.position);
        bool isClose = IsCloseEnough(targetDistance, distanceToKillPlayer);

        if (isClose && navMeshAgent.pathStatus == NavMeshPathStatus.PathComplete)
        {
            PlayerManager.Instance.DisableControls();
            EnemySpawnManager.Instance.OnCatchPlayer();
            fadePanel.Fade(fadeTime);
        }
    }

    void OnScreenBlacked()
    {
        CheckpointManager.Instance.RespawnPlayer();
        EnemySpawnManager.Instance.OnAferCatch();
    }

    void OnFadeFinish()
    {
        PlayerManager.Instance.EnableControls();
        EnemySpawnManager.Instance.ableToSpawn = true;
        EnemySpawnManager.Instance.StartFollowing();
    }

    bool IsCloseEnough(Vector3 magnitude, float distance)
    {
        Debug.Log(Mathf.Pow(distance, 2f));
        return Mathf.Abs(magnitude.sqrMagnitude) <= Mathf.Pow(distance, 2f);
    }
}
