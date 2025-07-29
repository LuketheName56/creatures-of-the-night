using System;
using UnityEngine;
using Unity.Cinemachine;

public class CameraZone : MonoBehaviour
{
    public CinemachineCamera cam;
    public int priority = 10;
    public bool activateOnStart = false;
    
    private void Start()
    {
        cam.Priority = activateOnStart ? priority : 0;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            cam.Priority = priority;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            cam.Priority = 0;
        }
    }
}
