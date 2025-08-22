using UnityEngine;

public class ItemManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    int orbCount = 0;

    /*
    void IncrementOrbCount(int orbAmount)
    {
        orbCount += orbAmount;
        //update UI elemnt to = orbCount
    }
    */
    public void CollectItem(CollectableSOBase collectable)
    {
        orbCount += collectable.OrbAmount;
        Debug.Log(orbCount + " orbs");
    }
    
    private void OnEnable() { Collectable.AddCollectObserver(CollectItem); }
    private void OnDisable() { Collectable.RemoveCollectObserver(CollectItem); }
    
}
