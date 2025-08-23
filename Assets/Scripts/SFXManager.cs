using UnityEngine;

public class SFXManager : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;

    public void PlaySound(AudioClip audioClip)
    {
        Debug.Log("orbSound");
        audioSource.clip = audioClip;
        audioSource.Play();

        // if (audioSource != null && audioSource.clip != null)
        // {
        //     audioSource.clip = audioClip;
        //     audioSource.Play();
        // }
        // else Debug.LogWarning("Audio clip is null");
    }

}
