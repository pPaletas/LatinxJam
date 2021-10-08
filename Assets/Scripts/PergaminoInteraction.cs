using UnityEngine;
using UnityEngine.UI;

public class PergaminoInteraction : MonoBehaviour
{
    private bool isPlaying;
    private GameObject entryObject;
    [SerializeField] private AudioSource entryAudio;
    
    //Para el imageholder colocar el image que esta dentro del CanvasPergamino
    [SerializeField] private GameObject scrollCanvas;
    [SerializeField] private Image imageHolder;
    [SerializeField] private AudioSource mainSceneAudioSource; //Colocar un main audio source en la escena y referenciarlo acá
    
    [Header("Pergaminos")] 
    [SerializeField] private Sprite pergaminoEspañol;
    [SerializeField] private Sprite pergaminoIngles;

    [Header("Sonidos")] 
    [SerializeField] private AudioClip clipEspañol;
    [SerializeField] private AudioClip clipIngles;

    private bool alreadyCollected = false;

    private void Awake()
    {
        entryObject = GameObject.FindGameObjectWithTag("EntryObject");
    }

    private void Update()
    {
        if (isPlaying && !mainSceneAudioSource.isPlaying)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                StopScrollInteraction();
                PlayerManager.Instance.EnableControls();
                OpenEntry();
            }
            
        }
    }

    void OpenEntry() {
        if (PlayerManager.Instance.GetCollectedScrolls() >= PlayerManager.Instance.scrollsToOpen)
        {
            Destroy(entryObject);
            entryAudio.Play();
        }
    }

    void StartScrollInteraction()
    {
        scrollCanvas.SetActive(true);
        
        isPlaying = true;
        
        LanguageManager.Language actualLanguage = LanguageManager.Instance.GetActualLanguage();
        if (actualLanguage == LanguageManager.Language.INGLES)
        {
            mainSceneAudioSource.clip = clipIngles;
            mainSceneAudioSource.Play();
            imageHolder.sprite = pergaminoIngles;
        }
        else
        {
            mainSceneAudioSource.clip = clipEspañol;
            mainSceneAudioSource.Play();
            imageHolder.sprite = pergaminoEspañol;
        }

        PlayerManager.Instance.DisableControls();
    }

    private void StopScrollInteraction()
    {
        isPlaying = false;
        scrollCanvas.SetActive(false);
        mainSceneAudioSource.Stop();
    }

    private void AddCollectedScroll()
    {
        if (!alreadyCollected)
        {
            alreadyCollected = true;
            StartScrollInteraction();
            PlayerManager.Instance.AddCollectedScroll();
        }
        
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.E) && !isPlaying)
            {
                AddCollectedScroll();
            }
        }
    }
    


}
