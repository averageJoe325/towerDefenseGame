//  Handles the expplosion resistant enemy.
public class ExplosionResistantEnemy : Enemy
{
    // Set protected values.
    private void Awake()
    {
        immunities.Add(Projectile.DamageType.Explosive);
        speed = 100;
        health = 4;
        moneyOnDeath = 5;
    }
}
