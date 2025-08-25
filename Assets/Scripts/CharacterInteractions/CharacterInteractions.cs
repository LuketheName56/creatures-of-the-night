using UnityEngine;

public class CharacterInteractions : MonoBehaviour, IDamagable
{
    public int characterHealth = 3;

    public void TakeDamage(int damageAmount)
    {
        characterHealth -= damageAmount;
        Debug.Log("characterHealth: " + characterHealth);
    }

    void Update()
    {
        
        //if a colision happens
        //get the damageAmount 
        //damage the player by damageAmount
    }
}
