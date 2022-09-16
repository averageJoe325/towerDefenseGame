//  Handles the strong enemy.
public class StrongEnemy : Enemy
{
    // Set protected values.
    private void Awake()
    {
        speed = 100;
        health = 5;
        moneyOnDeath = 5;
    }
}