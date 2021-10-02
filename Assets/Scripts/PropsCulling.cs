using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropsCulling : MonoBehaviour
{
    [SerializeField] float cullingRadius;
    [SerializeField] Camera playerCamera;

    Transform[] cullingObjects;
    CullingGroup cullingGroup;

    private void Awake()
    {
        FindCullingObjects();
        SetCullingUp();
    }

    private void OnDestroy() {
        cullingGroup.Dispose();
    }

    void FindCullingObjects()
    {
        Transform objectsParent = GameObject.Find("Props").transform;
        cullingObjects = new Transform[objectsParent.childCount];

        for (int p = 0; p < objectsParent.childCount; p++)
        {
            Transform child = objectsParent.GetChild(p);

            child.gameObject.SetActive(false);//Por defecto todos estan inactivos
            cullingObjects[p] = child;
        }
    }

    void SetCullingUp()
    {
        cullingGroup = new CullingGroup();
        cullingGroup.targetCamera = playerCamera;

        cullingGroup.SetBoundingSpheres(GetSpheres());
        cullingGroup.SetBoundingSphereCount(cullingObjects.Length);

        cullingGroup.onStateChanged += HideNonVisible;
    }

    BoundingSphere[] GetSpheres()
    {
        BoundingSphere[] boundingSpheres = new BoundingSphere[1000];        

        for (int o = 0; o < cullingObjects.Length; o++)
        {
            boundingSpheres[o] = new BoundingSphere(cullingObjects[o].position,cullingRadius);
        }

        return boundingSpheres;
    }

    void HideNonVisible(CullingGroupEvent evt)
    {
        cullingObjects[evt.index].gameObject.SetActive(evt.isVisible);
    }

    private void OnDrawGizmos() {
        if(!Application.isPlaying) return;

        for (int i = 0; i < cullingObjects.Length; i++)
        {
            Gizmos.DrawWireSphere(cullingObjects[i].position,cullingRadius);
        }
    }

}