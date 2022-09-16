using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastTower : Tower
{
    //  Set protected values.
    private void Awake()
    {
        price = 1800;
        upgradesText = new string[] { "Slowdown\n($1300)", "Hot Bullets\n($500)" };
        upgradesPrice = new int[] { 1300, 500 };
        range = 300;
        fireRate = .15f;
        projectile = "Projectiles/FastProjectile/Base";
        projectileSpeed = 900;
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
                projectile = upgrades[1] ? "Projectiles/FastProjectile/11" : "Projectiles/FastProjectile/1x";
                break;
            case 1:
                projectile = upgrades[0] ? "Projectiles/FastProjectile/11" : "Projectiles/FastProjectile/x1";
                break;
        }
    }
}