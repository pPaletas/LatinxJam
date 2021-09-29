using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventInteraction : MonoBehaviour
{
    RawImage rawImage;
    const float FADEDURATION = 0.4f;

    private void Awake() {
        rawImage = GetComponent<RawImage>();

        rawImage.CrossFadeAlpha(0f,0f,true);//Hace al panel invisible
    }

    public void Fade(float secs)
    {
        StartCoroutine(AFade(secs));
    }

    IEnumerator AFade(float secs)
    {
        rawImage.CrossFadeAlpha(1f,FADEDURATION,true);
        yield return new WaitForSeconds(FADEDURATION + secs);//Espera a que el Fade termine, pantalla negra por secs Segundos
        rawImage.CrossFadeAlpha(0f,FADEDURATION,true);
    }
}