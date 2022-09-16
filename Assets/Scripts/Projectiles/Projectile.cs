using System.Collections.Generic;
using UnityEngine;

//  Defines how projectiles behave after firing.
public abstract class Projectile : MonoBehaviour
{
    public enum DamageType { Basic, Explosive, Claw };
    public DamageType damageType { get; protected set; }
    public Vector2 velocity;
    protected float damage;
    protected float armorDamage;
    protected float knockback;
    protected int pierce = 1;
    protected float lifespan;
    protected float[] statusEffects = new float[1];
    protected float timer;

    //  Move the projectile.
    protected virtual void Update()
    {
        GetComponent<RectTransform>().localPosition += (Vector3)velocity * Time.deltaTime;
        if (timer > lifespan) { Destroy(gameObject); }
        timer += Time.deltaTime;
    }

    //  Destroy the projectile after collision with enemy.
    protected virtual void OnTriggerEnter2D(Collider2D col)
    {
        if (pierce > 0 && col.gameObject.GetComponent<Enemy>() != null)
        {
            col.gameObject.GetComponent<Enemy>().TakeDamage(damage, armorDamage, damageType);
            col.gameObject.GetComponent<Enemy>().TakeKnockback(knockback, damageType);
            col.gameObject.GetComponent<Enemy>().GiveStatus(statusEffects);
            pierce--;
            if (pierce == 0)
                Destroy(gameObject);
        }
    }
}