public class ArmoredEnemy : Enemy
{
    // Set protected values.
    private void Awake()
    {
        speed = 60;
        health = 5;
        armor = 5;
        moneyOnDeath = 6;
    }
}