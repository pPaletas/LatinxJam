using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : InteractableObject
{
    public override void OnVisionEnter()
    {
        transform.GetChild(0).gameObject.SetActive(true);
    }

    public override void OnVisionLeave()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, RadiusToTrigger);
    }

}
