using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventInteraction : MonoBehaviour
{
    InteractableObject[] interactableObjects;
    InteractableObject currentObject;

    private void Awake()
    {
        interactableObjects = FindObjectsOfType<InteractableObject>();
    }

    private void Update()
    {
        LookForInteractableObject();

        if (currentObject != null)
        {
            FireInteractableObject();
        }
    }

    void LookForInteractableObject()
    {
        for (int i = 0; i < interactableObjects.Length; i++)
        {
            Vector3 objectPosition;
            bool isInRadius = IsInRadius(interactableObjects[i], out objectPosition);

            //Si el jugador se encuentra en el rango del objeto, primero verifico posición para conocer el objeto mas cercano
            if (interactableObjects[i].IsInteractable && isInRadius)
            {
                //Solo se activa si actualmente no se encuentra dentro del rango de otro objeto
                bool isInSight = IsInSight(interactableObjects[i], objectPosition);

                if (currentObject == null && isInSight)
                {
                    currentObject = interactableObjects[i];
                    currentObject.OnVisionEnter();
                }
                else if (currentObject == interactableObjects[i] && !isInSight)
                {
                    currentObject.OnVisionLeave();
                    currentObject = null;
                }
            }
            //este caso solo ocurre si el objeto de este indice es currentObject
            //Si ya no está en el rango o ya no es interactuable
            else if (currentObject == interactableObjects[i] && (!isInRadius || !currentObject.IsInteractable))
            {
                if (currentObject.IsInteractable) currentObject.OnVisionLeave();
                currentObject = null;
            }
        }
    }

    bool IsInRadius(InteractableObject eventObject, out Vector3 position)
    {
        position = Vector3.zero;//Establece Vector3.zero por defecto

        Vector3 objectPosition = eventObject.transform.position;
        objectPosition = new Vector3(objectPosition.x, transform.position.y, objectPosition.z);//La misma altura del jugador

        if ((Vector3.SqrMagnitude(transform.position - objectPosition) <= Mathf.Pow(eventObject.RadiusToTrigger, 2f)))
        {
            position = objectPosition;
            return true;
        }
        else return false;
    }

    bool IsInSight(InteractableObject eventObject, Vector3 objectPosition)
    {
        float magnitude = Vector3.SqrMagnitude(transform.position - objectPosition);
        Vector3 unit = (objectPosition - transform.position).normalized;//Vector entre el jugador y el objeto
        float dot = Vector3.Dot(transform.forward.normalized, unit);//Angulo de visión en relación al unit
        float cosAngle = Mathf.Cos(eventObject.AngleToTrigger * Mathf.Deg2Rad);

        if (dot >= cosAngle) return true;
        else return false;
    }

    void FireInteractableObject()
    {
        if (Input.GetKeyDown(currentObject.TriggerKey))
        {
            currentObject.OnTriggerKey();
        }
    }
}