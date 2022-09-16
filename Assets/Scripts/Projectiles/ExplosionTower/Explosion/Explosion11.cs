//  Handles explosions of the 11 explosive projectile.
public class Explosion11 : Explosion
{
    //  Set protected values.
    protected override void Awake()
    {
        damage = 2;
        armorDamage = 2;
        lifespan = 4 / 9f;
    }
}