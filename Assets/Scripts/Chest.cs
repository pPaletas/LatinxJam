using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : InteractableObject
{
    ParticleSystem particles;

    private void Awake()
    {
        particles = GetComponentInChildren<ParticleSystem>();
    }

    public override void OnVisionEnter()
    {
        particles.Play();
    }

    public override void OnVisionLeave()
    {
        particles.Stop();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, RadiusToTrigger);
    }

}
