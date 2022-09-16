//Handles the mass cannon.
public class MassCannonTower : Tower
{
    //  Set protected values.
    private void Awake()
    {
        price = 450;
        upgradesText = new string[] { "Increase Projectile Lifespan\n($230)", "Increase Fire Rate\n($240)" };
        upgradesPrice = new int[] { 230, 240 };
        range = 200;
        fireRate = 1.5f;
        projectile = "Projectiles/MassCannonProjectile/Base";
        projectileSpeed = 375;
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
                projectile = "Projectiles/MassCannonProjectile/1x";
                break;
            case 1:
                fireRate = 1f;
                break;
        }
    }
}