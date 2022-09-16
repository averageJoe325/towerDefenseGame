using UnityEngine;

//  Handles projectiles of the 11 explosion tower.
public class ExplosionProjectile11 : ExplosionProjectile
{
    //  Make explosions.
    protected override void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.GetComponent<Enemy>() != null)
        {
            Instantiate(Resources.Load("Prefabs/Projectiles/ExplosionProjectile/Explosion/11"), transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}