//  Handles explosions of the 1x explosive projectile.
public class Explosion1x : Explosion
{
    //  Set protected values.
    protected override void Awake()
    {
        damage = 1f;
        armorDamage = 1f;
        lifespan = 4 / 9f;
    }
}