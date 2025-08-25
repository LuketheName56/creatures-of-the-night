using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.Serialization;

[RequireComponent(typeof(BoxCollider2D))]
public class CameraTriggerZone : MonoBehaviour
{
    [SerializeField] private CameraManager cameraManager;
    [SerializeField] private CinemachineCamera camera;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("TriggerEnter2D");
        if (collision.TryGetComponent(out Character character))
        {
            Debug.Log("Character triggered");
            cameraManager.SwitchCamera(camera);
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("TriggerExit2D");
        if (collision.TryGetComponent(out Character character))
            cameraManager.RevertToDefaultCamera();
    }
}
