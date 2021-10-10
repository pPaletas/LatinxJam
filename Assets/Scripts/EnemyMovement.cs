using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyMovement : MonoBehaviour
{
    #region Singleton
    public static EnemyMovement Instance;
    #endregion

    private NavMeshAgent agent;
    private AudioSource persecutionAudio;
    private Transform target;
    [Header("Spawn")]
    [SerializeField] private GameObject enemyBody;
    [SerializeField] private float spawnAfter;
    [SerializeField] private float spawnRange;
    [SerializeField] private float spawnFreq;
    [SerializeField] private float aliveTime;
    [SerializeField] private float stoppingDistanceQuiet;
    [SerializeField] private float stoppingDistanceAggresive;
    [SerializeField] private int navMeshMask;
    private float twiceHeight;
    private int currentTries = 0;
    private bool isFollowing = false;
    private bool inCycle = false;

    private const int POSITIONTRIES = 10;

    private void Awake()
    {
        Instance = this;

        agent = GetComponent<NavMeshAgent>();
        persecutionAudio = GetComponentInChildren<AudioSource>();
        target = GameObject.FindGameObjectWithTag("Player").transform;

        twiceHeight = agent.height * 2f;
    }

    private void Update()
    {
        Follow();
    }

    public void StartCycle()
    {
        if (!inCycle) inCycle = true;
        StartCoroutine(AsyncStartCycle());
    }

    IEnumerator AsyncStartCycle()
    {
        while (inCycle)
        {
            yield return new WaitForSeconds(spawnAfter);
            isFollowing = true;
            CalculateNextPosition();//El inCycle solo puede ser desactivado aqui

            yield return new WaitForSeconds(aliveTime);
            if(!inCycle) break;
            isFollowing = false;
            UnManifest();

            yield return new WaitForSeconds(spawnFreq);
        }
    }

    void CalculateNextPosition()
    {
        Vector2 randomCircle = Random.insideUnitCircle;
        Vector3 nextPosition = new Vector3(randomCircle.x, 0f, randomCircle.y).normalized;
        nextPosition *= spawnRange;//Rango fijo
        nextPosition += target.position;//Rango alrededor del jugador

        NavMeshHit hit;

        if (NavMesh.SamplePosition(nextPosition, out hit, twiceHeight, navMeshMask))
        {
            Manifest(hit.position);
            currentTries = 0;
        }
        else if (currentTries < POSITIONTRIES)
        {
            currentTries++;
            CalculateNextPosition();
        }
        else
        {
            Debug.LogWarning("Closest NavMesh of mask " + navMeshMask + " is too far away");
        }
    }

    void Manifest(Vector3 position)
    {
        transform.position = position;
        enemyBody.SetActive(true);
        if (isFollowing && !persecutionAudio.isPlaying) persecutionAudio.Play();
    }

    void Follow()
    {
        if (isFollowing)
        {
            agent.isStopped = false;
            agent.destination = target.position;
        }
        else
        {
            agent.isStopped = true;
        }
    }

    public void UnManifest()
    {
        enemyBody.SetActive(false);
        if (persecutionAudio.isPlaying) persecutionAudio.Stop();
    }

    public bool IsEnemyClose(float magnitude)
    {
        Vector3 distanceDiff = target.position - transform.position;
        return Mathf.Abs(distanceDiff.sqrMagnitude) <= Mathf.Pow(magnitude,2f) && agent.pathStatus == NavMeshPathStatus.PathComplete && isFollowing;
    }

    public bool IsManifested() => enemyBody.activeSelf;

    public void EndCycle()
    {
        inCycle = false;//Termina el ciclo.
        isFollowing = false;
    }

    public void MakeAggresive()
    {
        agent.stoppingDistance = stoppingDistanceAggresive;
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Gizmos.DrawWireSphere(target.position,spawnRange);
        }
    }

    #region Static Methods
    public static void IntantiateEnemy(NavMeshAgent enemyPrefab, Vector3 position)
    {
        NavMeshHit hit;

        if (NavMesh.SamplePosition(position, out hit, enemyPrefab.height * 2, 3))
        {
            NavMeshAgent clone = Instantiate<NavMeshAgent>(enemyPrefab, hit.position, enemyPrefab.transform.rotation);
            clone.Warp(hit.position);
            clone.transform.GetChild(0).localPosition = Vector3.zero;
        }
        else
        {
            Debug.LogWarning("Enemy isn´t close enough to NavMesh");
        }
    }
    #endregion
}
