using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuManager : MonoBehaviour
{

    [SerializeField] private float duration;
    [SerializeField] private GameObject[] texts;
    [SerializeField] private RawImage img;
    

    private void Awake()
    {
        img.CrossFadeAlpha(0, duration, false);

        PlayerManager.Instance.DisableControls();
    }

    public void StartGame()
    {
        PlayerManager.Instance.EnableControls();
    }

    public void Settings()
    {
        if(LanguageManager.Instance.GetActualLanguage() == LanguageManager.Language.INGLES)
        {
            LanguageManager.Instance.ChangeLanguageSpanish();
        }
        else
        {
             LanguageManager.Instance.ChangeLanguageEnglish();
        }
        ReloadLanguage();
    }

    private void ReloadLanguage()
    {
        for (int t = 0; t < texts.Length; t++)
        {
            texts[t].SetActive(!texts[t].activeSelf);
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
