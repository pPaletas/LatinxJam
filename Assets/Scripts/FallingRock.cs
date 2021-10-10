using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingRock : MonoBehaviour
{
    private static List<FallingRock> rocks = new List<FallingRock>();

    [SerializeField]private float warningTime;
    [SerializeField]private float stopTime;
    private bool shouldFall = false;

    private Rigidbody rigidbody_;
    private Vector3 origPosition;

    private void Awake()
    {
        rigidbody_ = GetComponent<Rigidbody>();
        origPosition = transform.position;
        rocks.Add(this);
    }

    public void Fall()
    {
        shouldFall = true;
        StartCoroutine(AsyncFall());
    }

    IEnumerator AsyncFall()
    {
        yield return new WaitForSeconds(warningTime);
        if(shouldFall) rigidbody_.isKinematic = false;
        yield return new WaitForSeconds(stopTime);
        rigidbody_.isKinematic = true;
    }

    public static void RestartRocks()
    {
        for (int r = 0; r < rocks.Count; r++)
        {
            rocks[r].shouldFall = false;
            rocks[r].rigidbody_.isKinematic = true;
            rocks[r].transform.position = rocks[r].origPosition;
        }
    }
}
