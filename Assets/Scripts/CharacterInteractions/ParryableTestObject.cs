using UnityEngine;

public class ParryableTestObject : MonoBehaviour, IParryable
{
    public void Parry()
    {
        Debug.Log("Oh man, I got parried!");
    }
}
