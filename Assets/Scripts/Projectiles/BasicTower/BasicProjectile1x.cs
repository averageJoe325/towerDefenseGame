//  Handles projectiles of 1x base tower.
public class BasicProjectile1x : BasicProjectile
{
    //  Fix lifespan.
    protected override void Awake()
    {
        base.Awake();
        lifespan = 1.3f / 3;
    }
}