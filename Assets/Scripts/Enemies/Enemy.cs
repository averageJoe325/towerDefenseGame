using System.Collections.Generic;
using UnityEngine;

//  Defines Enemy behavior.
public abstract class Enemy : MonoBehaviour
{
    public enum Status { Slowdown }
    public float health { get; protected set; }
    public float armor { get; protected set; }
    protected float speed;
    protected int moneyOnDeath;
    protected List<Projectile.DamageType> immunities = new List<Projectile.DamageType>();

    public bool targetable { get; private set; }
    protected int position = 1;
    protected Vector2? knockbackDest;
    protected float speedMultiplier { get; private set; } = 1;
    private float[] statusEffects = new float[1];

    //  Setup path and spawn point.
    private void Start()
    {
        GetComponent<RectTransform>().localPosition = GameManager.singleton.path[0];
        targetable = true;
    }

    //  Reset status effects, see what status effects need to be applied, and run the code.
    protected virtual void Update()
    {
        print(DistanceToTravel());
        speedMultiplier = 1;
        for (int i = 0; i < statusEffects.Length; i++)
        {
            statusEffects[i] = Mathf.Max(statusEffects[i] - Time.deltaTime, 0);
            if (statusEffects[i] > 0)
            {
                switch (i)
                {
                    case 0:
                        ApplyStatus(Status.Slowdown);
                        break;
                }
            }
        }
        Move();
    }

    //  Take damage.
    public virtual void TakeDamage(float damage, float armorDamage, Projectile.DamageType damageType)
    {
        if (immunities.Contains(damageType)) { return; }
        armor = Mathf.Max(armor - armorDamage, 0);
        if (armor >= damage)
            armor -= damage;
        else
        {
            health -= damage - armor;
            armor = 0;
        }
        if (health <= 0) { Die(); }
    }

    //Take knockback.
    public Vector2? TakeKnockback(float knockback, Projectile.DamageType damageType)
    {
        if (Mathf.Abs(knockback) < Mathf.Epsilon) { return null; }
        Vector2 prev = GameManager.singleton.path[position - 1];
        Vector2 dest = GetComponent<RectTransform>().localPosition;
        float distToPrev = Vector2.Distance(dest, prev);
        int origPosition = position;
        while (distToPrev < knockback)
        {
            dest = prev;
            knockback -= distToPrev;
            position--;
            if (position <= 0) { break; }
            prev = GameManager.singleton.path[position - 1];
            distToPrev = Vector2.Distance(dest, prev);
        }
        if (position > 0) { dest += knockback * (prev - dest).normalized; }
        if (immunities.Contains(damageType))
            position = origPosition;
        else
        {
            knockbackDest = dest;
            position = Mathf.Max(position, 1);
        }
        return (dest - (Vector2)GetComponent<RectTransform>().localPosition).normalized;
    }

    //  Give status effects.
    public void GiveStatus(float[] effects)
    {
        for (int i = 0; i < statusEffects.Length; i++)
            statusEffects[i] = Mathf.Max(statusEffects[i], effects[i]);
    }

    //  Apply status effects.
    private void ApplyStatus(Status status)
    {
        switch (status)
        {
            case Status.Slowdown:
                speedMultiplier = 0.5f;
                break;
        }
    }

    //  Die and give the player money.
    private void Die()
    {
        GameManager.singleton.money += moneyOnDeath;
        Destroy(gameObject);
    }


    //  Move the enemy along the track.
    protected virtual void Move()
    {
        Vector2 dest = knockbackDest == null ? GameManager.singleton.path[position] : (Vector2)knockbackDest;
        Vector2 dir = (dest - (Vector2)GetComponent<RectTransform>().localPosition).normalized;
        GetComponent<RectTransform>().localPosition += (Vector3)dir * Time.deltaTime * speed * speedMultiplier;
        if (Vector2.Dot((dest - (Vector2)GetComponent<RectTransform>().localPosition).normalized, dir) <= 0)
        {
            GetComponent<RectTransform>().localPosition = dest;
            if (knockbackDest != null)
                knockbackDest = null;
            else if (position < GameManager.singleton.path.Count - 1)
                position++;
            else
                ReachExit();
        }
    }

    //  Find how much distance the enemy will need to cover.
    public float DistanceToTravel()
    {
        float dist = Vector2.Distance(GetComponent<RectTransform>().localPosition, GameManager.singleton.path[position]);
        for (int i = position; i < GameManager.singleton.path.Count - 1; i++)
            dist += Vector2.Distance(GameManager.singleton.path[i], GameManager.singleton.path[i + 1]);
        return dist;
    }

    //  Exit the map and remove lives.
    private void ReachExit()
    {
        GameManager.singleton.lives--;
        Destroy(gameObject);
    }
}
