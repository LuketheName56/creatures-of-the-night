using UnityEngine;
using UnityEngine.Serialization;


[CreateAssetMenu(menuName = "Collectables/CollectableOrbSO", fileName = "Orb")]
public class CollectableOrbSO : CollectableSOBase
{
    [Header("Collectable Stats")]
    public int OrbAmount = 1;

    public override void Collect(GameObject objectCollected)
    {
        //make this an event?
        //ItemManager.instance.IncrementOrbCount(OrbAmount);
    }

}
