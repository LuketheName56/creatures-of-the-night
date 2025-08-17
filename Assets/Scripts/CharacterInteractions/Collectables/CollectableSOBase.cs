using UnityEngine;

public abstract class CollectableSOBase : ScriptableObject
{
    [Header("CollectionFX")] //can be overriden in derived SOs
    public AudioClip CollectClip;
    public float CollectFlashTime;

    //requires any class inheriting from SOBase to implement this method
    public abstract void Collect(GameObject objectCollected);
}
