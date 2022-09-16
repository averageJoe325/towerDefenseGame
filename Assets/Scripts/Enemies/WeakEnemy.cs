//  Handles the weak enemy.
public class WeakEnemy : Enemy
{
    // Set protected values.
    private void Awake()
    {
        speed = 60;
        health = 2;
        moneyOnDeath = 2;
    }
}