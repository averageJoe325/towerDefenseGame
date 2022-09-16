using UnityEngine;

//  Handles the claw projectiles.
public class ClawProjectile : Projectile
{
    public Vector2 startLocation { get; private set; }
    private float hasHitTarget = -1;
    private float grabLength = .125f;
    private bool retracting;

    //  Set protected values.
    protected virtual void Awake()
    {
        damageType = DamageType.Claw;
        knockback = 200;
        lifespan = .5f;
        startLocation = GetComponent<RectTransform>().localPosition;
    }

    //  Move the claw correctly.
    protected override void OnTriggerEnter2D(Collider2D col)
    {
        if (pierce > 0 && !retracting && col.gameObject.GetComponent<Enemy>() != null)
        {
            col.gameObject.GetComponent<Enemy>().TakeDamage(damage, armorDamage, damageType);
            Vector2? dir = (Vector2)col.gameObject.GetComponent<Enemy>().TakeKnockback(knockback, damageType);
            if (hasHitTarget < 0 && dir != null)
            {
                velocity = (Vector2)dir * velocity.magnitude;
                hasHitTarget = 0;
            }
            pierce--;
        }
    }

    //  Make the claw retract after a set amount of time.
    protected override void Update()
    {
        if (startLocation == Vector2.zero)
            startLocation = GetComponent<RectTransform>().localPosition;
        GetComponent<RectTransform>().localPosition += (Vector3)velocity * Time.deltaTime;
        if ((timer > lifespan && hasHitTarget < 0) || hasHitTarget > grabLength)
        {
            if (Vector2.Dot((startLocation - (Vector2)GetComponent<RectTransform>().localPosition).normalized, velocity) <= 0 && retracting)
                Destroy(gameObject);
            velocity = (startLocation - (Vector2)GetComponent<RectTransform>().localPosition).normalized * velocity.magnitude;
            retracting = true;
        }
        timer += Time.deltaTime;
        if (hasHitTarget >= 0)
            hasHitTarget += Time.deltaTime;
    }
}
