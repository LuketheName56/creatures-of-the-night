using UnityEngine;

public abstract class CollectableSOBase : ScriptableObject
{
    //requires any class inheriting from SOBase to implement this method
    public abstract void Collect(GameObject objectCollected);
}
