using UnityEngine;
using System.Collections.Generic;

//  Handles the dumb tower
public class DumbTower : Tower
{
    //  Set protected values.
    private void Awake()
    {
        price = 240;
        upgradesText = new string[] { "Less Small Brain\n($80)", "Increase Fire Rate\n($110)" };
        upgradesPrice = new int[] { 80, 110 };
        range = 150;
        fireRate = .5f;
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
                break;
            case 1:
                fireRate = .35f;
                break;
        }
    }

    //  Shoots at random.
    protected override void Shoot()
    {
        fireTimer = 0;
        GameObject go = (GameObject)Instantiate(Resources.Load($"Prefabs/{projectile}"), GameObject.Find("GamePanel").transform);
        go.GetComponent<RectTransform>().localPosition = transform.localPosition;
        go.GetComponent<Projectile>().velocity = projectileSpeed * RandomVector(Random.Range(0f, 1f));
    }

    //  Get a random angle. If upgraded, make it hit track.
    private Vector2 RandomVector(float value)
    {
        if (!upgrades[0])
        {
            float theta = value * 2 * Mathf.PI;
            return new Vector2(Mathf.Cos(theta), Mathf.Sin(theta));
        }

        Vector2 pos = GetComponent<RectTransform>().localPosition;
        print($"Position: {pos}.");
        List<float> segmentLength = new List<float>();
        List<Vector2[]> lines = new List<Vector2[]>();
        float totalLength = 0;
        for (int i = 1; i < GameManager.singleton.path.Count; i++)
        {
            Vector2[] line = { GameManager.singleton.path[i - 1], GameManager.singleton.path[i] };
            print($"Old line from {line[0]} to {line[1]}.");
            Vector2 dir = (line[1] - line[0]).normalized;
            Vector2 closestPoint = line[0] + Vector2.Dot(pos - line[0], dir) * dir;
            print($"Closest point: {closestPoint}.");
            if (Vector2.Distance(closestPoint, pos) > range) { continue; }
            for (int j = 0; j < 2; j++)
            {
                if (Vector2.Distance(pos, line[j]) > range)
                {
                    line[j] = closestPoint + (line[j] - closestPoint).normalized * Mathf.Sqrt(Mathf.Pow(range, 2) - Mathf.Pow(Vector2.Distance(closestPoint, pos), 2));
                }
            }
            segmentLength.Add(Vector2.Distance(line[0], line[1]));
            lines.Add(line);
            totalLength += segmentLength[segmentLength.Count - 1];
            print($"Line from {line[0]} to {line[1]}.");
        }
        value *= totalLength;
        int lineIndex = 0;
        while (value > segmentLength[lineIndex])
        {
            value -= segmentLength[lineIndex];
            lineIndex++;
        }
        Vector2[] targetLine = lines[lineIndex];
        Vector2 target = targetLine[0] + (targetLine[1] - targetLine[0]) * (value / segmentLength[lineIndex]);
        return (target - pos).normalized;
    }
}