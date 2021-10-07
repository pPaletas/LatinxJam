using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//????????????????????????Hacer que el arbol no haga parte de los InteractableObject

public class InteractableTree : InteractableObject
{
    [SerializeField] private Camera plrCamera;
    [SerializeField] private Vector3 fallenTreeRotation;
    [SerializeField] private AudioSource fallingTreeSound;

    public override void OnVisionEnter()
    {
        //Highlight PARTICULAS???
    }

    public override void OnVisionLeave()
    {
        //Disable Hightlight
    }

    public override void OnTriggerKey()
    {
        PlayerManager.Instance.goodEnding = false;
        FadePanel.Instance.Fade(4f);
        FadePanel.Instance.OnBlackScreen.AddListener(TreeChop);
        FadePanel.Instance.OnComplete.AddListener(GetPlayerMovementBack);

        //Disable Highlight
        IsInteractable = false;//No más interacciones
    }

    void TreeChop()
    {
        if (fallingTreeSound != null) fallingTreeSound.Play();//Suena si existe audioclip
        transform.rotation = Quaternion.Euler(fallenTreeRotation);

        PlayerManager.Instance.DisableControls();
        PlayerManager.Instance.LookAt(transform.GetChild(0).position);//Mira hacia el puente
    }

    void GetPlayerMovementBack()
    {
        PlayerManager.Instance.EnableControls();
    }
}
