using UnityEngine;

public class LanguageManager : MonoBehaviour
{
    //Colocar este script en un gameObject vacio en la escena del menú principal
    
    public static LanguageManager Instance;
    
    public enum Language
    {
        ESPAÑOL,
        INGLES
    }

    private void Start()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private Language actualLanguage;

    public Language GetActualLanguage()
    {
        return actualLanguage;
    }

    public void ChangeLanguageSpanish()
    {
        actualLanguage = Language.ESPAÑOL;
    }

    public void ChangeLanguageEnglish()
    {
        actualLanguage = Language.INGLES;
    }
}
