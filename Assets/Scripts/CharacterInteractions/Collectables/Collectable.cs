using System;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
// [RequireComponent(typeof(CollectableTriggerHandler))]
public class Collectable : MonoBehaviour
{
    private GameObject player;
    [SerializeField] private CollectableSOBase collectableSO;
    public static event Action<CollectableSOBase> OnCollect;

    public static void AddCollectObserver(Action<CollectableSOBase> observer) { OnCollect += observer; }
    public static void RemoveCollectObserver(Action<CollectableSOBase> observer) { OnCollect -= observer; }
    
    private void Awake()
    {
        //may replace w/ TryGetComponent
        player = GameObject.FindGameObjectWithTag("Player");
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == player)
        {
            Debug.Log("collected");
            OnCollect?.Invoke(collectableSO);
            Destroy(gameObject); //self destruct
        }
    }
    private void Reset()
    {
        GetComponent<CircleCollider2D>().isTrigger = true;
    }
    
}