//  Handles the explosion tower.
public class ExplosionTower : Tower
{
    //  Set protected values.
    private void Awake()
    {
        price = 600;
        upgradesText = new string[] { "Bigger Explosion\n($600)", "Increased Damage\n($710)" };
        upgradesPrice = new int[] { 600, 710 };
        range = 150;
        fireRate = 1.5f;
        projectile = "Projectiles/ExplosionProjectile/Base";
        projectileSpeed = 300;
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
                projectile = upgrades[1] ? "Projectiles/ExplosionProjectile/11" : "Projectiles/ExplosionProjectile/1x";
                break;
            case 1:
                projectile = upgrades[0] ? "Projectiles/ExplosionProjectile/11" : "Projectiles/ExplosionProjectile/x1";
                break;
        }
    }
}