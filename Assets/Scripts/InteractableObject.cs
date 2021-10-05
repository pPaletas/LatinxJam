using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableObject : MonoBehaviour
{
    public bool IsInteractable{get; set;} = true;
    [field:SerializeField]
    public KeyCode TriggerKey{get; private set;}
    [field:SerializeField]
    public float RadiusToTrigger{get; private set;}
    [field:SerializeField]
    public float AngleToTrigger{get; private set;}

    public virtual void OnTriggerKey(){}
    public virtual void OnVisionEnter(){}
    public virtual void OnVisionLeave(){}
}
