using System;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class Collectable : MonoBehaviour
{
    private GameObject player;
    [SerializeField] private CollectableSOBase collectableSO;
    
    private SFXManager sfxManager;
    private ItemManager itemManager;
    
    private void Awake()
    {
        //may replace w/ TryGetComponent
        player = GameObject.FindGameObjectWithTag("Player");
        
        sfxManager = ServiceLocator.Get<SFXManager>();
        itemManager = ServiceLocator.Get<ItemManager>();
    }
    //private void Collect()
    //refactor??
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == player)
        {
            Debug.Log("collected");
            sfxManager.PlaySound(collectableSO.CollectClip);
            itemManager.Collect(collectableSO);
            Destroy(gameObject); //self destruct
        }
    }
    private void Reset()
    {
        GetComponent<CircleCollider2D>().isTrigger = true;
    }
    
}