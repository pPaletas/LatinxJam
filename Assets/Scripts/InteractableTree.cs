using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//????????????????????????Hacer que el arbol no haga parte de los InteractableObject

public class InteractableTree : InteractableObject
{
    [SerializeField] private Camera plrCamera;
    [SerializeField] private AudioSource fallingTreeSound;
    [SerializeField] private FadePanel fallingTreePanel;

    [SerializeField] private Vector3 fallenTreeRotation;

    public override void OnVisionEnter()
    {
        //Enable Highlight
        transform.GetChild(1).gameObject.SetActive(true);
    }

    public override void OnVisionLeave()
    {
        //Disable Highlight
        transform.GetChild(1).gameObject.SetActive(false);
    }

    public override void OnTriggerKey()
    {
        PlayerManager.Instance.goodEnding = false;
        fallingTreePanel.Fade(0.4f,4);
        fallingTreePanel.OnBlackScreen.AddListener(TreeChop);
        fallingTreePanel.OnComplete.AddListener(GetPlayerMovementBack);

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
