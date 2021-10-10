using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CollisionInteractions : MonoBehaviour
{
    [SerializeField] private NavMeshAgent enemyPrefab;

    private FadePanel riverDeathPanel;

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("River"))
        {
            CheckpointManager.Instance.RespawnPlayer();
            FallingRock.RestartRocks();
        }
        else if (hit.gameObject.CompareTag("Rock"))
        {
            hit.gameObject.GetComponent<FallingRock>().Fall();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("EnemyStarter"))
        {
            other.enabled = false;
            EnemyMovement.IntantiateEnemy(enemyPrefab, transform.position);
            EnemyMovement.Instance.StartCycle();
        }
        else if (other.gameObject.CompareTag("AggressiveState"))
        {
            // EnemySpawnManager.Instance.MakeAgressive(!PlayerManager.Instance.goodEnding);
        }
        else if (other.gameObject.CompareTag("CheckPoint"))
        {
            CheckpointManager.Instance.SetTransformAsCheckpoint(other.transform);
        }
    }
}