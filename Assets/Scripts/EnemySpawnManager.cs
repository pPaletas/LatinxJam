using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//---------------ESTE SCRIPT AÚN TIENE QUE SER TESTEADO EN UN RELIEVE
public class EnemySpawnManager : MonoBehaviour
{
    #region Singleton
    public static EnemySpawnManager Instance { get; private set; }
    #endregion

    public bool ableToSpawn = true;

    private EnemyMovement enemyMovement;
    [SerializeField] private GameObject body;

    [SerializeField] private float twiceHeight = 10f;
    [SerializeField] private float spawnRadius = 5f;
    [SerializeField] private float spawnAfter = 20f;
    [SerializeField] private float spawnFreqMin = 1f;
    [SerializeField] private float spawnFreqMax = 1f;
    [SerializeField] private float aliveTimeMin = 1f;
    [SerializeField] private float aliveTimeMax = 1f;
    [SerializeField] private float aggressiveSpeedMultiplier = 2f;

    bool alreadySpawned = false;
    bool alreadyAggressive = false;

    private void Awake()
    {
        Instance = this;
        enemyMovement = GetComponent<EnemyMovement>();
    }

    public void StartFollowing()
    {
        if(alreadySpawned) return;
        alreadySpawned = true;
        StartCoroutine(AsyncStartFollowing());
    }

    public void MakeAgressive(bool aggresive)
    {
        if(alreadyAggressive) return;
        alreadyAggressive = true;
        //Preguntar
        
        enemyMovement.navMeshAgent.speed *= aggressiveSpeedMultiplier;
    }

    private IEnumerator AsyncStartFollowing()
    {
        yield return new WaitForSeconds(spawnAfter);

        while (true)
        {
            if (ableToSpawn)
            {
                ChangeAliveState();//Lives
                yield return new WaitForSeconds(Random.Range(aliveTimeMin,aliveTimeMax));
                ChangeAliveState();//Dies
                yield return new WaitForSeconds(Random.Range(spawnFreqMin,spawnFreqMax));
            }
        }
    }

    void ChangeAliveState()
    {
        while (!enemyMovement.canMove && !SetRandomPosition()) ;//Esperando a que la madremonte encuentre posición

        enemyMovement.canMove = !enemyMovement.canMove;
        body.SetActive(enemyMovement.canMove);//Desaparece el cuerpo
    }

    bool SetRandomPosition()
    {
        //Radio alrededor del jugador
        Vector3 randomPosition = enemyMovement.movePositionTransform.position + GetRandomPositionOnRadius();

        NavMeshHit hit;
        bool samplePosition = NavMesh.SamplePosition(randomPosition, out hit, twiceHeight * 2f, 3);

        if (samplePosition)//Si encuentra una posición cercana
        {
            transform.position = hit.position;
            return transform;
        }

        return samplePosition;
    }

    Vector3 GetRandomPositionOnRadius()
    {
        Vector2 circleRandom = Random.insideUnitSphere;
        Vector3 position = new Vector3(circleRandom.x,0f,circleRandom.y).normalized;

        return circleRandom * spawnRadius;
    }

    private void OnDrawGizmos() {
        if(!Application.isPlaying) return;

        Gizmos.DrawWireSphere(enemyMovement.movePositionTransform.position,spawnRadius);
    }
}
