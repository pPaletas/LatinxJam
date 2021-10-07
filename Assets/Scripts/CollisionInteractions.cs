using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionInteractions : MonoBehaviour
{
    [SerializeField]private AudioSource fallingRockAudio;
    private Rigidbody currentRigidbody;

    [SerializeField]private float timeTillShake;
    [SerializeField]private float timeTillFall;
    [SerializeField]private float timeTillStop;

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Rock") && hit.rigidbody != currentRigidbody)
        {
            currentRigidbody = hit.rigidbody;
            StartCoroutine(AsyncFall(hit.rigidbody));
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("EnemyStarter"))
        {
            EnemySpawnManager.Instance.StartFollowing();
        }
        else if (other.gameObject.CompareTag("AggressiveState"))
        {
            EnemySpawnManager.Instance.MakeAgressive(!PlayerManager.Instance.goodEnding);
        }
    }

    IEnumerator AsyncFall(Rigidbody rigidbody_)
    {
        yield return new WaitForSeconds(timeTillShake);
        //NECESITO ITWEEN PARA SHAKE
        if(fallingRockAudio != null) fallingRockAudio.Play();
        yield return new WaitForSeconds(timeTillFall);
        rigidbody_.isKinematic = false;
        yield return new WaitForSeconds(timeTillStop);
        rigidbody_.isKinematic = true;
        if(fallingRockAudio != null) fallingRockAudio.Stop();
    }
}