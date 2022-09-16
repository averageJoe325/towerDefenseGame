//  Handles the basic enemy.
public class BasicEnemy : Enemy
{
    // Set protected values.
    private void Awake()
    {
        speed = 100;
        health = 3;
        moneyOnDeath = 3;
    }
}