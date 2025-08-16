using UnityEngine;

public class CollectableTriggerHandler : MonoBehaviour
{
    private void Awake()
    {
        //may replace w/ TryGetComponent
        collectableSO = GetComponent<Collectable>;
        player = GameObject.FindWithTag("Player");
    }

    /* I don't want to use layermask, since the COLLECTABLE is 
    calling the Collect method when the PLAYER runs into it. 
    Reference to player should not require LayerMask. 
    [SerializeFeild] private LayerMask collectableLayer; 
    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((1 << collision.other.layer) == collectableLayer.value)
        {

        }

    }
    */

    /* optional helper function
        public static bool ObjectInLayerMask(GameObject gameObject, LayerMask layerMask)
        {
            if ((layerMask.value))
        }
    */

    private void OnTriggerEnter2D(Collider2D other)
    {   //?
        if (other.GameObject = player)
        {
            collectableSO.Collect(collision.gameObject);
        }

    }
}
