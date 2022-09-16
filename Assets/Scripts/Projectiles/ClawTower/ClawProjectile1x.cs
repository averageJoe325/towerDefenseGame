//  Handles the projectiles of the 1x claw tower.
public class ClawProjectile1x : ClawProjectile
{
    // Increase knockback.
    protected override void Awake()
    {
        base.Awake();
        knockback = 350;
    }
}