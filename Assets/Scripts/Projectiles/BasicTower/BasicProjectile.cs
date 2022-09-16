//  Handles basic projectiles.
public class BasicProjectile : Projectile
{
    //  Set protected values.
    protected virtual void Awake()
    {
        damage = 1;
        lifespan = 2 / 9f;
    }
}