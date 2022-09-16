//  Handles explosions of the x1 explosive projectile.
public class Explosionx1 : Explosion
{
    //  Set protected values.
    protected override void Awake()
    {
        damage = 2;
        armorDamage = 2;
        lifespan = 1 / 3f;
    }
}