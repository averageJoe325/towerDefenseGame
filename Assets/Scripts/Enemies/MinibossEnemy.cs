//  Handles the miniboss enemy.
public class MinibossEnemy : Enemy
{
    // Set protected values.
    private void Awake()
    {
        speed = 80;
        health = 10;
        armor = 2;
        moneyOnDeath = 11;
    }
}