using UnityEngine;

//  Handles explosion projectiles.
public class ExplosionProjectile : Projectile
{
    //  Set protected values.
    protected virtual void Awake()
    {
        lifespan = 1.6f / 3;
    }

    //  Make explosions.
    protected override void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.GetComponent<Enemy>() != null)
        {
            Instantiate(Resources.Load("Prefabs/Projectiles/ExplosionProjectile/Explosion/Base"), transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}