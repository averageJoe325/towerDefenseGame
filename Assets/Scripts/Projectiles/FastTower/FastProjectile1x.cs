public class FastProjectile1x : FastProjectile
{
    protected override void Awake()
    {
        base.Awake();
        statusEffects[0] = 1;
    }
}