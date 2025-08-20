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
    void IncrementOrbCount(CollectableSOBase collectable)
    {
        orbCount+= collectable.orbCount;
        Debug.Log(orbCount + " orbs");
    }
        
    
    private void OnEnable()
    {
        Collectable.AddCollectObserver(IncrementOrbCount);
    }

    private void OnDisable()
    {
        Collectable.RemoveCollectObserver(IncrementOrbCount);
    }

}
