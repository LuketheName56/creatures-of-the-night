using UnityEngine;
[CreateAssetMenu(menuName = "Collectables/CollectableSOBase", fileName = "BasicCollectable")]

public abstract class CollectableSOBase : ScriptableObject
{
    // [Header("Collectable Stats")]

    [Header("CollectionFX")] //can be overriden in derived SOs?? but not actually??????????????
    [SerializeField] public AudioClip CollectClip;
    
    //requires any class inheriting from SOBase to implement this method
    // public abstract void Collect();
}
