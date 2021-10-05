using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionInteractions : MonoBehaviour
{

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Rock"))
        {
            //hit.gameObject.GetComponent<FallingRock>().Fall();
        }

    }

}
