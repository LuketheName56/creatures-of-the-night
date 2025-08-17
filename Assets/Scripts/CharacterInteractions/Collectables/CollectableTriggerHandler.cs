using UnityEngine;

public class CollectableTriggerHandler : MonoBehaviour
{
    private GameObject player;
    private Collectable _collectable;

    private void Awake()
    {
        //may replace w/ TryGetComponent
        player = GameObject.FindGameObjectWithTag("Player");
        _collectable = GetComponent<Collectable>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {   //?
        if (other.gameObject == player)
        {
            _collectable.Collect(other.gameObject);
            Destroy(gameObject); //self destruct??
        }
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


}
