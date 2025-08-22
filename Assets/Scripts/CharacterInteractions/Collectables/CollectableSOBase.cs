using UnityEngine;
[CreateAssetMenu(menuName = "Collectables/CollectableSOBase", fileName = "Base")]

public abstract class CollectableSOBase : ScriptableObject
{
    [Header("Collectable Stats")]
    public int OrbAmount = 0;
    
    [Header("CollectionFX")] //can be overriden in derived SOs
    [SerializeField] public AudioClip CollectClip;
    
    //requires any class inheriting from SOBase to implement this method
    //public abstract void Collect(GameObject objectCollected);
}
