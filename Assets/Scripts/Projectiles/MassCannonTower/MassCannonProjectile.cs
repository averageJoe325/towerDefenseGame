using UnityEngine;

//  Handles mass cannon projectiles.
public class MassCannonProjectile : Projectile
{
    //  Set protected values.
    protected virtual void Awake()
    {
        damage = 1;
        lifespan = 2 / 3f;
    }

    //  Create piercing effect
    protected override void OnTriggerEnter2D(Collider2D col)
    {
        if (pierce > 0 && col.gameObject.GetComponent<Enemy>() != null)
        {
            col.gameObject.GetComponent<Enemy>().TakeDamage(damage, armorDamage, damageType);
            col.gameObject.GetComponent<Enemy>().TakeKnockback(knockback, damageType);
        }
    }
}