using UnityEngine;

public class ItemManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    int orbCount = 0;

    void IncrementOrbCount(int orbAmount)
    {
        orbCount += orbAmount;
    }

}
