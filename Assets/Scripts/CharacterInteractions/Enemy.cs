using UnityEngine;

public class Enemy : MonoBehaviour
{
    private int enemyStrength = 1;
    void Update()
    {
        //enemy behaviour
    }

    public void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log("collision Occured");
        IDamagable damagable = col.collider.GetComponent<IDamagable>();
        if (damagable != null)
            damagable.TakeDamage(enemyStrength);
    }
}
