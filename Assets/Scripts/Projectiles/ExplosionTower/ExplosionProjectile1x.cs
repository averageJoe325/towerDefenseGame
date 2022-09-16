using UnityEngine;

//  Handles projectiles of the 1x explosion tower.
public class ExplosionProjectile1x : ExplosionProjectile
{
    //  Make bigger explosions.
    protected override void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.GetComponent<Enemy>() != null)
        {
            Instantiate(Resources.Load("Prefabs/Projectiles/ExplosionProjectile/Explosion/1x"), transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}