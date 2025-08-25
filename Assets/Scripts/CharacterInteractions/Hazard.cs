using UnityEngine;

public class Hazard : MonoBehaviour
{
    private int hazardStrength = 2;

    public void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log("collision Occured");
        IDamagable damagable = col.collider.GetComponent<IDamagable>();
        if (damagable != null)
            damagable.TakeDamage(hazardStrength);
    }
}
