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

    [SerializeField] private GameObject body;

    [SerializeField] private float spawnRadius = 5f;
    [SerializeField] private float spawnAfter = 20f;
    [SerializeField] private float spawnFreqMin = 1f;
    [SerializeField] private float spawnFreqMax = 1f;
    [SerializeField] private float aliveTimeMin = 1f;
    [SerializeField] private float aliveTimeMax = 1f;
    [SerializeField] private float aggressiveSpeedMultiplier = 2f;

    private EnemyMovement enemyMovement;
    private AudioSource enemyAudio;

    private bool alreadySpawned = false;
    private bool alreadyAggressive = false;
    private int triesNumber = 10;
    private float twiceHeight;

    private void Awake()
    {
        Instance = this;
        enemyMovement = GetComponent<EnemyMovement>();
        enemyAudio = GetComponent<AudioSource>();
    }

    private void Start()
    {
        twiceHeight = enemyMovement.navMeshAgent.height * 2f;//Lo recomendable según Unity
    }

    public void StartFollowing()
    {
        if (alreadySpawned) return;
        alreadySpawned = true;
        enemyMovement.movePositionTransform = GameObject.FindGameObjectWithTag("Player").transform;

        StartCoroutine(AsyncStartFollowing());
    }

    public void MakeAgressive(bool aggresive)
    {
        if (alreadyAggressive) return;
        alreadyAggressive = true;
        //Preguntar

        enemyMovement.navMeshAgent.speed *= aggressiveSpeedMultiplier;
    }

    private IEnumerator AsyncStartFollowing()
    {
        yield return new WaitForSeconds(spawnAfter);

        while (ableToSpawn)
        {
            ChangeAliveState();//Lives
            yield return new WaitForSeconds(Random.Range(aliveTimeMin, aliveTimeMax));
            if (ableToSpawn)
            {
                ChangeAliveState();//Dies
                yield return new WaitForSeconds(Random.Range(spawnFreqMin, spawnFreqMax));
            }
        }

        alreadySpawned = false;//Deshabilitar el spawn
    }

    void ChangeAliveState()
    {
        if (!enemyMovement.canMove)
        {
            WaitForPosition();
        }
        else
        {
            enemyAudio.Stop();
        }

        enemyMovement.canMove = !enemyMovement.canMove;
        body.SetActive(enemyMovement.canMove);//Desaparece el cuerpo
    }

    public void OnCatchPlayer()
    {
        ableToSpawn = false;
        enemyMovement.canMove = false;
        PlayerManager.Instance.LookAt(transform.GetChild(1).position);//Mira a los ojos
    }

    public void OnAferCatch()
    {
        enemyMovement.canMove = true;
        ChangeAliveState();//Desaparece
    }

    void WaitForPosition()
    {
        bool foundPosition = SetRandomPosition();
        int currentTries = 0;

        while (!foundPosition)
        {
            foundPosition = SetRandomPosition();

            if (currentTries < triesNumber) currentTries++;
            else
            {
                Debug.LogWarning("Unable to spawn Madremonte, make sure player isn't too far from Navmesh");
                enemyMovement.canMove = true;//Me aseguro de que la madremonte no spawnee
                return;
            }
        }

        enemyAudio.Play();//Solo suena si encontró posición
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
        Vector3 position = new Vector3(circleRandom.x, 0f, circleRandom.y).normalized;

        return position * spawnRadius;
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;

        Gizmos.DrawWireSphere(enemyMovement.movePositionTransform.position, spawnRadius);
    }
}
