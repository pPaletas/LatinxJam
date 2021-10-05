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
    [SerializeField] private GameObject interactionUI;

    public override void OnVisionEnter()
    {
        //Highlight
    }

    public override void OnVisionLeave()
    {
        //Disable Hightlight
    }

    public override void OnTriggerKey()
    {
        FadePanel.Instance.Fade(4f);
        FadePanel.Instance.OnBlackScreen.AddListener(TreeChop);
        interactionUI.SetActive(false);//Desaparecer efecto de Highlight

        IsInteractable = false;//No más interacciones
    }

    void TreeChop()
    {
        //PlayerManager.Instance.DisableControls();
        //PlayerManager.Instance.LookAt(transform.position);
        if (fallingTreeSound != null) fallingTreeSound.Play();//Suena si existe audioclip
        transform.rotation = Quaternion.Euler(fallenTreeRotation);
        plrCamera.transform.LookAt(transform.position);
    }
}
