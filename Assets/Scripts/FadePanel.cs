using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class FadePanel : MonoBehaviour
{
    #region Singleton
    public static FadePanel Instance;
    #endregion

    RawImage rawImage;
    const float FADEDURATION = 0.4f;

    public UnityEvent OnBlackScreen;
    public UnityEvent OnComplete;

    private void Awake() {
        Instance = this;

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
        yield return new WaitForSeconds(FADEDURATION);

        OnBlackScreen.Invoke();
        yield return new WaitForSeconds(secs);

        rawImage.CrossFadeAlpha(0f,FADEDURATION,true);
        yield return new WaitForSeconds(FADEDURATION);
        OnComplete.Invoke();
    }
}
