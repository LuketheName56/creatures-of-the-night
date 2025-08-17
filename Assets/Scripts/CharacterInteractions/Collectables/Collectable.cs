using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(CollectableTriggerHandler))]
public class Collectable : MonoBehaviour
{
    [SerializeField] private CollectableSOBase collectableSO;
    private void Reset()
    {
        GetComponent<CircleCollider2D>().isTrigger = true;
    }

//what does this do if it has no implementation and 0 refrences?
    public void Collect(GameObject objectCollected)
    {

    }

}