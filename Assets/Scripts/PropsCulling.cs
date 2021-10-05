using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropsCulling : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;

    [SerializeField] private float cullingRadius;
    [SerializeField] private int cullingObjectsLayer;

    List<GameObject> cullingObjects = new List<GameObject>();
    CullingGroup cullingGroup;

    private void Awake()
    {
        FindObjectsInLayer(cullingObjectsLayer);
        SetCullingUp();
    }

    void FindObjectsInLayer(int layer)
    {
        GameObject[] allGameObjects = GameObject.FindObjectsOfType<GameObject>();

        for (int g = 0; g < allGameObjects.Length; g++)
        {
            if (allGameObjects[g].layer == cullingObjectsLayer) cullingObjects.Add(allGameObjects[g]);
        }
    }

    void SetCullingUp()
    {
        cullingGroup = new CullingGroup();
        cullingGroup.targetCamera = playerCamera;

        cullingGroup.SetBoundingSpheres(GetSpheres());
        cullingGroup.SetBoundingSphereCount(cullingObjects.Count);

        cullingGroup.onStateChanged += HideNonVisible;
    }

    BoundingSphere[] GetSpheres()
    {
        BoundingSphere[] boundingSpheres = new BoundingSphere[1000];

        for (int o = 0; o < cullingObjects.Count; o++)
        {
            boundingSpheres[o] = new BoundingSphere(cullingObjects[o].transform.position, cullingRadius);
        }

        return boundingSpheres;
    }

    void HideNonVisible(CullingGroupEvent evt)
    {
        cullingObjects[evt.index].gameObject.SetActive(evt.isVisible);
    }

    private void OnDestroy()//Siempre hay que asegurarse de hacer Dispose al destruir el objeto.
    {   
        cullingGroup.Dispose();
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;

        for (int i = 0; i < cullingObjects.Count; i++)
        {
            Gizmos.DrawWireSphere(cullingObjects[i].transform.position, cullingRadius);
        }
    }

}