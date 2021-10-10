using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class FadePanel : MonoBehaviour
{
    RawImage rawImage;

    public UnityEvent OnBlackScreen;
    public UnityEvent OnComplete;

    private void Awake()
    {
        rawImage = GetComponent<RawImage>();
        rawImage.CrossFadeAlpha(0f, 0f, true);//Hace al panel invisible
    }

    public void Fade(float fadeTime, float blackTime)
    {
        StartCoroutine(AsyncFade(fadeTime,blackTime));
    }

    IEnumerator AsyncFade(float fadeTime,float blackTime)
    {
        rawImage.CrossFadeAlpha(1f, fadeTime, true);
        yield return new WaitForSeconds(fadeTime);

        OnBlackScreen.Invoke();
        yield return new WaitForSeconds(blackTime);

        rawImage.CrossFadeAlpha(0f, fadeTime, true);
        yield return new WaitForSeconds(fadeTime);
        OnComplete.Invoke();
    }
}
