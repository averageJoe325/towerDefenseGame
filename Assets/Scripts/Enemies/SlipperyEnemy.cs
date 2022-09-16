//  Handles the slippery enemy.
public class SlipperyEnemy : Enemy
{
    // Set protected values,
    private void Awake()
    {
        immunities.Add(Projectile.DamageType.Claw);
        speed = 150;
        health = 3;
        moneyOnDeath = 5;
    }
}