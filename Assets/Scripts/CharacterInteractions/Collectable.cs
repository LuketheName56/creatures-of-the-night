using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(CollectableTriggerHandler))]
public class Collectable : MonoBehaviour
{
    [SerializeFeild] private CollectableSOBase collectableSO;
    private void Reset()
    {
        GetComponent<CircleCollider2D>().isTrigger = true;
    }

    public void Collect(GameObject objectCollected)
    {
        
    }

}