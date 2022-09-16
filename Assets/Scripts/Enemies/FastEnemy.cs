//  Handles the fast enemy.
public class FastEnemy : Enemy
{
    // Set protected values.
    private void Awake()
    {
        speed = 250;
        health = 3;
        moneyOnDeath = 8;
    }
}