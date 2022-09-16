//  Handles the basic tower.
public class BasicTower : Tower
{
    //  Set protected values.
    private void Awake()
    {
        price = 200;
        upgradesText = new string[] { "Increase Range\n($140)", "Increase Fire Rate\n($110)" };
        upgradesPrice = new int[] { 140, 110 };
        range = 150;
        fireRate = 1;
        projectile = "Projectiles/BasicProjectile/Base";
        projectileSpeed = 600;
    }

    //  Manage upgrades
    public override void Upgrade(int path)
    {
        if (upgradesPrice[path] > GameManager.singleton.money || upgrades[path]) { return; }
        GameManager.singleton.money -= upgradesPrice[path];
        base.Upgrade(path);
        switch (path)
        {
            case 0:
                range = 250;
                projectile = "Projectiles/BasicProjectile/1x";
                break;
            case 1:
                fireRate = .75f;
                break;
        }
    }
}