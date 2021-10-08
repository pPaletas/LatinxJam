using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    [HideInInspector] public Transform movePositionTransform;
    [HideInInspector] public NavMeshAgent navMeshAgent;
    public bool canMove = false;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (canMove)
        {
            if (navMeshAgent.isStopped) navMeshAgent.isStopped = false;
            navMeshAgent.destination = movePositionTransform.position;
        }
        else if (!navMeshAgent.isStopped)
        {
            navMeshAgent.isStopped = true;
        }
    }
}
