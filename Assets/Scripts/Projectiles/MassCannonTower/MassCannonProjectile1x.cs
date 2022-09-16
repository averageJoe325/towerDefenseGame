//  Handles projectiles of 1x mass cannon.
public class MassCannonProjectile1x : MassCannonProjectile
{
    //  Fix lifespan
    protected override void Awake()
    {
        base.Awake();
        lifespan = 2.5f / 3;
    }
}
