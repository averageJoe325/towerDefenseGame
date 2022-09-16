using UnityEngine;

//  Manage armor health bar graphics
public class ArmorBar : HealthBar
{
    //  Set values.
    protected override void Awake()
    {
        enemy = transform.GetComponentInParent<Enemy>();
        maxHealth = enemy.armor;
        redBarRenderer = transform.GetComponent<SpriteRenderer>();
        greenBarRenderer = transform.GetChild(0).GetComponentInChildren<SpriteRenderer>();
        greenBarRect = transform.GetChild(0).GetComponent<RectTransform>();
    }

    //  Make the bar length correct.
    protected override void Update()
    {
        float ratio = enemy.armor / maxHealth;
        if (ratio < 1)
        {
            HealthBarUpdate(ratio);
            enemy.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
            enemy.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
        }
    }
}