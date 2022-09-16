//  Handles the fast tower's projectiles.
public class FastProjectile : Projectile
{
    //  Set protected values.
    protected virtual void Awake()
    {
        damage = 1;
        lifespan = 1 / 3f;
    }
}
