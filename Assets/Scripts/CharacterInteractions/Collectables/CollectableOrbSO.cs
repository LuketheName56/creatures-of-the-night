using UnityEngine;

[CreateAssetMenu(menuName = "Collectables/CollectableOrbSO", fileName = "Orb")]
public class CollectableOrbSO : CollectableSOBase
{
    [Header("Collectable Stats")]
    public int CurrencyAmount = 1;

    public override void Collect(GameObject objectCollected)
    {
        //make this an event?
        CurrencyManager.instance.IncrementCurrency(CurrencyAmount);
    }

}
