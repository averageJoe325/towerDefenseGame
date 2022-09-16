//  Handles the claw tower.
public class ClawTower : Tower
{
    //  Set protected values.
    private void Awake()
    {
        price = 850;
        upgradesText = new string[] { "Further Knockback\n($670)", "IncreasedFireRate\n($470)" };
        upgradesPrice = new int[] { 670, 470 };
        range = 250;
        fireRate = 3;
        projectile = "Projectiles/ClawProjectile/Base";
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
                projectile = "Projectiles/ClawProjectile/1x";
                break;
            case 1:
                fireRate = 2f;
                break;
        }
    }
}