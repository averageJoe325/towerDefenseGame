using UnityEngine;

//  Handles projectiles of the x1 explosion tower.
public class ExplosionProjectilex1 : ExplosionProjectile
{
    //  Make explosions.
    protected override void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.GetComponent<Enemy>() != null)
        {
            Instantiate(Resources.Load("Prefabs/Projectiles/ExplosionProjectile/Explosion/x1"), transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
