using UnityEngine;

//  Handles explosions.
public class Explosion : Projectile
{
    //  Set protected values.
    protected virtual void Awake()
    {
        damageType = DamageType.Explosive;
        damage = 1;
        armorDamage = 1;
        lifespan = 1 / 3f;
    }

    // Expand the explosion.
    protected override void Update()
    {
        base.Update();
        GetComponent<RectTransform>().localScale = 3 * timer * new Vector2(1, 1);
    }

    //  Don't destroy explosion.
    protected override void OnTriggerEnter2D(Collider2D col)
    {
        if (pierce > 0 && col.gameObject.GetComponent<Enemy>() != null)
        {
            col.gameObject.GetComponent<Enemy>().TakeDamage(damage, armorDamage, damageType);
            col.gameObject.GetComponent<Enemy>().TakeKnockback(knockback, damageType);
        }
    }
}