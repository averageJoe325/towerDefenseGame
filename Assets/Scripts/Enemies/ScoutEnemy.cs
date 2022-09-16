using System.Collections.Generic;
using UnityEngine;

//  Handles the scout enemy.
public class ScoutEnemy : Enemy
{
    private List<float> damages = new List<float>();
    private List<float> armorDamages = new List<float>();
    private List<Projectile.DamageType> damageTypes = new List<Projectile.DamageType>();
    private List<float> delays = new List<float>();
    private float timer;

    // Set protected values.
    private void Awake()
    {
        speed = 175;
        health = 4;
        moneyOnDeath = 4;
    }

    //  Create intel struct.
    public struct Intel
    {
        public List<float> Damage { get; internal set; }
        public List<float> ArmorDamage { get; internal set; }
        public List<Projectile.DamageType> DamageType { get; internal set; }
        public List<float> Delay { get; internal set; }

        internal Intel(List<float> damage, List<float> armorDamage, List<Projectile.DamageType> damageType, List<float> delay)
        {
            Damage = damage;
            ArmorDamage = armorDamage;
            DamageType = damageType;
            Delay = delay;
        }

        //  Return the average damage dealth, choosing whether or not to count armor damage.
        public float AverageDamage(bool useArmor)
        {
            float totalDamage = 0;
            for (int i = 0; i < Damage.Count; i++)
            {
                totalDamage += Damage[i];
                if (useArmor) { totalDamage += ArmorDamage[i]; }
            }
            return totalDamage / Damage.Count;
        }

        //  Return the average time between attacks, choosing whether or not to include 0 damage attacks.
        public float AverageDelay(bool useZeroDamage)
        {
            if (useZeroDamage) { return Delay[Delay.Count - 1] / Delay.Count; }
            int totalAttacks = 0;
            foreach (float damage in Damage)
                if (damage > 0) { totalAttacks++; }
            return Delay[Delay.Count - 1] / totalAttacks;
        }

        //  Return the proportion of a specific damage type dealth, choosing whether or not to count armor damage.
        public float DamageProportion(Projectile.DamageType damageType, bool useArmor)
        {
            float totalDamage = 0;
            float damageOfType = 0;
            for (int i = 0; i < Damage.Count; i++)
            {
                float damage = Damage[i];
                if (useArmor) { damage += ArmorDamage[i]; }
                totalDamage += damage;
                if (damageType == DamageType[i]) { damageOfType += damage; }
            }
            return damageOfType / totalDamage;
        }

        //  Return the proportion of attacks of a specific damage type.
        public float AttackProportion(Projectile.DamageType damageType)
        {
            int attacks = 0;
            foreach (Projectile.DamageType dt in DamageType)
                if (dt == damageType) { attacks++; }
            return (float)attacks / Damage.Count;
        }

        //  Return the proportion of armor damage compared to the total of both damage and armor damage.
        public float ArmorProportion()
        {
            float totalDamage = 0;
            float armorDamage = 0;
            for (int i = 0; i < Damage.Count; i++)
            {
                totalDamage += Damage[i];
                armorDamage += ArmorDamage[i];
            }
            return armorDamage / (totalDamage + armorDamage);
        }
    }

    //  Allow for intels to be combined.
    public static (Intel, int) CombineIntel(List<Intel> intels)
    {
        if (intels.Count == 1) { return (intels[0], 1); }
        if (intels.Count == 0) { return (new Intel(), 0); }

        //  Set up variables that track how far along in each Intel the program is.
        float timer = 0;
        int[] positions = new int[2];

        //  Create Lists to create the new Intel.
        List<float> damagesNew = new List<float>();
        List<float> armorDamagesNew = new List<float>();
        List<Projectile.DamageType> damageTypesNew = new List<Projectile.DamageType>();
        List<float> delaysNew = new List<float>();

        //  Concatenate Intels.
        while (true)
        {
            //  Choose the first section of the first Intel.
            int i = 0;
            if (positions[0] >= intels[0].Delay.Count)
            {
                if (positions[1] >= intels[1].Delay.Count) { break; }
                i = 1;
            }
            else if (positions[1] < intels[1].Delay.Count && intels[1].Delay[positions[1]] < intels[0].Delay[positions[0]])
                i = 1;

            //  Add the info to the Lists for the new Intel.
            damagesNew.Add(intels[i].Damage[positions[i]]);
            armorDamagesNew.Add(intels[i].ArmorDamage[positions[i]]);
            damageTypesNew.Add(intels[i].DamageType[positions[i]]);
            delaysNew.Add(intels[i].Delay[positions[i]]);

            //  Reset the timer and position.
            timer += intels[i].Delay[positions[i]];
            positions[i]++;
        }

        //  Create a new Intel list and pass it into the function again.
        Intel newIntel = new Intel(damagesNew, armorDamagesNew, damageTypesNew, delaysNew);
        List<Intel> newList = new List<Intel>();
        newList.Add(newIntel);
        for (int i = 2; i < intels.Count; i++)
        {
            newList.Add(intels[i]);
        }
        return (CombineIntel(newList).Item1, newList.Count + 1);
    }


    //  Allow for the delay timer to count.
    protected override void Update()
    {
        base.Update();
        timer += Time.deltaTime;
    }


    //  Gain intel.
    public override void TakeDamage(float damage, float armorDamage, Projectile.DamageType damageType)
    {
        base.TakeDamage(damage, armorDamage, damageType);
        damages.Add(damage);
        armorDamages.Add(armorDamage);
        damageTypes.Add(damageType);
        delays.Add(timer);
    }

    //  Give intel.
    private void GiveIntel()
    {
        GameManager.singleton.intelList.Add((new Intel(damages, armorDamages, damageTypes, delays), GameManager.singleton.round));
        Destroy(gameObject);
    }

    //  Allow for retreats.
    protected override void Move()
    {
        if (health > 2) { base.Move(); }
        else
        {
            Vector2 dest = knockbackDest == null ? GameManager.singleton.path[position - 1] : (Vector2)knockbackDest;
            Vector2 dir = (dest - (Vector2)GetComponent<RectTransform>().localPosition).normalized;
            GetComponent<RectTransform>().localPosition += (Vector3)dir * Time.deltaTime * speed * speedMultiplier;
            if (Vector2.Dot((dest - (Vector2)GetComponent<RectTransform>().localPosition).normalized, dir) <= 0)
            {
                GetComponent<RectTransform>().localPosition = dest;
                if (knockbackDest != null)
                    knockbackDest = null;
                else if (position > 1)
                    position--;
                else
                    GiveIntel();
            }
        }
    }
}
