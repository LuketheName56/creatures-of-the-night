using UnityEngine;

public class ItemManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    int orbCount = 0;

    public void Collect(CollectableSOBase collectable)
    {
        if (collectable is CollectableOrbSO orb)
        {
            IncrementOrbCount(orb.OrbAmount);
        }
    }
    
    void IncrementOrbCount(int orbAmount)
    {
        orbCount += orbAmount;
        //update UI elemnt to = orbCount
    }
}