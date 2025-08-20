using UnityEngine;

public class SFXManager : MonoBehaviour
{
    [SerializeField] private AudioClip orbSound;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void OnEnable() { Collectable.PlayCollectableSound(orbSound); }

    private void OnDisable() { Collectable.PlayCollectableSound(orbSound); }

    PlayCollectableSound(AudioClip orbSound)
    {
        Debug.Log("orbSound");
    }
}
