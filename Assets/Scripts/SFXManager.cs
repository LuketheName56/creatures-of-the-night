using UnityEngine;

public class SFXManager : MonoBehaviour
{
    [SerializeField] private CollectableSOBase collectableSOBase;
    private AudioSource audioSource;

    private void PlaySound(CollectableSOBase collectableSOBase)
    {
        Debug.Log("orbSound");
        if (audioSource != null && audioSource.clip != null)
        {
            collectableSOBase.CollectClip = audioSource.clip;
            audioSource.Play();
        }
    }
    
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    
    private void OnEnable() { Collectable.AddCollectObserver(PlaySound); }
    private void OnDisable() { Collectable.RemoveCollectObserver(PlaySound); }


    //private void OnDisable() { Collectable.PlayCollectableSound(orbSound); }

}
