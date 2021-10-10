using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInteraction : MonoBehaviour
{
    public bool isAttacking;

    [SerializeField] private float rangeToKill;
    [Header("Fade")]
    [SerializeField] private float fadeTime;
    [SerializeField] private float blackTime;
    private FadePanel deathFade;

    private void Awake()
    {
        deathFade = GameObject.Find("DeathFade").GetComponent<FadePanel>();
        deathFade.OnBlackScreen.AddListener(OnBlackScreen);
        deathFade.OnComplete.AddListener(OnFadeComplete);
    }

    private void Update()
    {
        if (EnemyMovement.Instance.IsManifested() && EnemyMovement.Instance.IsEnemyClose(rangeToKill))
        {
            Attack();
        }
    }

    void Attack()
    {
        PlayerManager.Instance.DisableControls();
        PlayerManager.Instance.LookAt(transform.GetChild(1).position);//Mira a los ojos

        EnemyMovement.Instance.EndCycle();
        deathFade.Fade(fadeTime, blackTime);
    }

    void OnBlackScreen()
    {
        EnemyMovement.Instance.UnManifest();
        CheckpointManager.Instance.RespawnPlayer();
    }

    void OnFadeComplete()
    {
        PlayerManager.Instance.EnableControls();
        EnemyMovement.Instance.StartCycle();
    }
}