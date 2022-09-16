using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//  Manages money and lives, placing towers, and spawning enemies.
public class GameManager : MonoBehaviour
{
    #region Variables
    public static GameManager singleton;

    public List<Vector2> path { get; private set; } = new List<Vector2> { new Vector2(-675, -180), new Vector2(0, -180), new Vector2(0, 90), new Vector2(-337.5f, 90), new Vector2(-337.5f, 360), new Vector2(337.5f, 360), new Vector2(337.5f, -540) };
    public List<(ScoutEnemy.Intel, int)> intelList = new List<(ScoutEnemy.Intel, int)>();
    public int round { get; private set; } = 2;
    private Round[] rounds;
    private int wavesActive;

    public int money = 550;
    public int lives = 50;
    private int moneyPerRound = 100;
    private TextMeshProUGUI roundText;
    private TextMeshProUGUI moneyText;
    private TextMeshProUGUI livesText;

    public GameObject currentTower;
    public GameObject selectedTower;
    public bool selected;
    private bool overMenu;
    #endregion


    #region Enemies
    //  Defines waves of enemies based on enemy type and count and spacing of wave.
    internal readonly struct Wave
    {
        internal string Enemy { get; }
        internal int Count { get; }
        internal float Spacing { get; }

        internal Wave(string enemy, int count, float spacing)
        {
            Enemy = enemy;
            Count = count;
            Spacing = spacing;
        }
    }

    //  Defines rounds based on waves and the spacing between them.
    internal readonly struct Round
    {
        internal Wave[] Waves { get; }
        internal float[] Delay { get; }

        internal Round(Wave[] waves, float[] delay)
        {
            Waves = waves;
            Delay = delay;
        }
    }

    //  Check if a round is still active.
    public bool IsRoundOver()
    {
        return wavesActive == 0 && FindObjectOfType<Enemy>() == null;
    }

    //  Spawn Rounds.
    public IEnumerator SpawnRound()
    {
        InfoText.singleton.ChangeTextState(false);
        if (round >= rounds.Length || !IsRoundOver()) { yield return null; }
        else
        {
            Round r;
            if (round < 20) { r = rounds[round]; }
            else { r = CreateRoundFromIntel(round); }
            for (int i = 0; i < r.Waves.Length; i++)
            {
                StartCoroutine(SpawnWave(r.Waves[i]));
                yield return new WaitForSeconds(r.Delay[i]);
            }
            yield return new WaitUntil(IsRoundOver);
            round++;
            money += moneyPerRound;
            InfoText.singleton.ChangeTextState(true);
        }
    }

    //  Spawn Waves
    private IEnumerator SpawnWave(Wave wave)
    {
        wavesActive++;
        for (int i = 0; i < wave.Count; i++)
        {
            Instantiate(Resources.Load($"Prefabs/Enemies/{wave.Enemy}"), GameObject.Find("GamePanel").transform);
            yield return new WaitForSeconds(wave.Spacing);
        }
        wavesActive--;
    }

    //  Create a round
    private Round CreateRoundFromIntel(int desiredRound)
    {
        List<ScoutEnemy.Intel> roundIntel = new List<ScoutEnemy.Intel>();
        foreach ((ScoutEnemy.Intel, int) intel in intelList)
            if (intel.Item2 < desiredRound - 5) { roundIntel.Add(intel.Item1); }
        (ScoutEnemy.Intel, int) totalIntel = ScoutEnemy.CombineIntel(roundIntel);
        List<Wave> waves = new List<Wave>();
        List<float> delay = new List<float>();
        switch (desiredRound)
        {
            case 20:
                waves.Add(new Wave("StrongEnemy", 15, .85f));
                delay.Add(.85f * 15);
                if (totalIntel.Item2 == 0 ||
                    totalIntel.Item1.AttackProportion(Projectile.DamageType.Explosive)
                    >= totalIntel.Item1.AttackProportion(Projectile.DamageType.Claw))
                    waves.Add(new Wave("ExplosionResistantEnemy", 13, .55f));
                else
                    waves.Add(new Wave("SlipperyEnemy", 8, 1));
                break;
            case 21:
                if (totalIntel.Item2 == 0 ||
                    totalIntel.Item1.ArmorProportion() >= .25f)
                    waves.Add(new Wave("ArmoredEnemy", 23, 1.15f));
                else
                {
                    waves.Add(new Wave("StrongEnemy", 10, .7f));
                    waves.Add(new Wave("FastEnemy", 9, 1));
                    delay.Add(.7f * 10);
                }
                break;
            case 22:
                if (totalIntel.Item2 == 0 ||
                    totalIntel.Item1.AttackProportion(Projectile.DamageType.Explosive)
                    >= totalIntel.Item1.AttackProportion(Projectile.DamageType.Claw))
                    waves.Add(new Wave("ExplosionResistantEnemy", 35, .35f));
                else
                    waves.Add(new Wave("SlipperyEnemy", 63, .6f));
                break;
            case 23:
                if (totalIntel.Item2 == 0 ||
                    totalIntel.Item1.AverageDelay(true) >= 1f)
                    waves.Add(new Wave("FastEnemy", 72, .6f));
                else
                    waves.Add(new Wave("StrongEnemy", 33, .34f));
                break;
            case 24:
                if (totalIntel.Item2 == 0 ||
                    totalIntel.Item1.AttackProportion(Projectile.DamageType.Explosive)
                    >= totalIntel.Item1.AttackProportion(Projectile.DamageType.Claw) &&
                    totalIntel.Item1.ArmorProportion() >= .25f)
                {
                    waves.Add(new Wave("ExplosionResistantEnemy", 45, .75f));
                    waves.Add(new Wave("ArmoredEnemy", 30, 1));
                    delay.Add(.75f * 45);
                }
                else if (totalIntel.Item1.AttackProportion(Projectile.DamageType.Explosive)
                    < totalIntel.Item1.AttackProportion(Projectile.DamageType.Claw) &&
                    totalIntel.Item1.ArmorProportion() >= .25f)
                {
                    waves.Add(new Wave("SlipperyEnemy", 51, 1));
                    waves.Add(new Wave("ArmoredEnemy", 30, 1));
                    delay.Add(1 * 51);
                }
                else if (totalIntel.Item1.AttackProportion(Projectile.DamageType.Explosive)
                    >= totalIntel.Item1.AttackProportion(Projectile.DamageType.Claw) &&
                    totalIntel.Item1.ArmorProportion() < .25f)
                {
                    waves.Add(new Wave("ExplosionResistantEnemy", 55, .75f));
                    waves.Add(new Wave("StrongEnemy", 60, .55f));
                    delay.Add(.75f * 55);
                }
                else
                {
                    waves.Add(new Wave("SlipperyEnemy", 58, .75f));
                    waves.Add(new Wave("StrongEnemy", 45, .55f));
                    delay.Add(.75f * 58);
                }
                break;
        }
        delay.Add(0);
        return new Round(waves.ToArray(), delay.ToArray());
    }
    #endregion


    #region UI
    //  Update the GUI based on the round and the amount of money and lives remaining.
    private void UpdateText()
    {
        roundText.text = $"Round:   {round + 1}";
        moneyText.text = $"Money: ${money}";
        livesText.text = $"Lives:    {lives}";
    }

    //  Change the Menu based on selected tower.
    private void ChangeMenu()
    {
        if ((!selected || selectedTower == null) && !transform.GetChild(4).gameObject.name.Contains("Tower"))
        {
            Destroy(transform.GetChild(4).gameObject);
            Instantiate(Resources.Load("Prefabs/UI/TowerGrid"), transform);
        }
        if (selected && selectedTower != null && !transform.GetChild(4).gameObject.name.Contains("Upgrade"))
        {
            Destroy(transform.GetChild(4).gameObject);
            Instantiate(Resources.Load("Prefabs/UI/UpgradeGrid"), transform);
        }
    }

    //  Change button behavior and color based on selected tower.
    private void ActivateUpgradeButtons()
    {
        if (selected && selectedTower != null && transform.GetChild(4).gameObject.name.Contains("Upgrade"))
        {
            for (int i = 0; i < 2; i++)
            {
                int j = i;
                Transform buttonTransform = transform.GetChild(4).GetChild(j + 1);
                Button button = buttonTransform.GetComponent<Button>();
                Tower tower = selectedTower.GetComponent<Tower>();
                button.onClick.AddListener(() => tower.Upgrade(j));
                buttonTransform.GetChild(0).GetComponent<TextMeshProUGUI>().text = tower.upgradesText[j];
                button.interactable = !tower.upgrades[j] && money >= tower.upgradesPrice[j];
                if (tower.upgrades[j])
                {
                    ColorBlock colorBlock = button.colors;
                    colorBlock.disabledColor = new Color(0, .7843137f, 0, .5019608f);
                    button.colors = colorBlock;
                }
            }
        }
    }
    #endregion


    #region Towers
    //  Destroys the current tower and reverts the variable to null.
    public void DestroyCurrentTower()
    {
        if (currentTower == null) { return; }
        Destroy(currentTower);
        currentTower = null;
    }



    //  Place Towers.
    private void PlaceTower()
    {
        if (Input.GetMouseButtonDown(0) && currentTower != null)
        {
            int cost = currentTower.GetComponent<Tower>().price;
            if (currentTower.GetComponent<Tower>().Place())
                money -= cost;
        }
    }
    #endregion


    #region Unity methods
    private void Start()
    {
        //  Set up singleton.
        if (singleton != null && singleton != this)
            Destroy(gameObject);
        singleton = this;

        //  Set up rounds.
        rounds = new Round[25];
        rounds[0] = new Round(new Wave[] { new Wave("WeakEnemy", 10, 1) }, new float[] { 0 });
        rounds[1] = new Round(new Wave[] { new Wave("WeakEnemy", 15, .85f) }, new float[] { 0 });
        rounds[2] = new Round(new Wave[] { new Wave("WeakEnemy", 6, 1.25f), new Wave("BasicEnemy", 3, 1) }, new float[] { 1.25f * 6, 0 });
        rounds[3] = new Round(new Wave[] { new Wave("WeakEnemy", 10, .85f), new Wave("BasicEnemy", 5, 1) }, new float[] { .85f * 10, 0 });
        rounds[4] = new Round(new Wave[] { new Wave("StrongEnemy", 3, 1.5f), new Wave("WeakEnemy", 5, 1) }, new float[] { 1.5f * 3, 0 });
        rounds[5] = new Round(new Wave[] { new Wave("BasicEnemy", 8, .75f) }, new float[] { 0 });
        rounds[6] = new Round(new Wave[] { new Wave("WeakEnemy", 4, 1), new Wave("BasicEnemy", 5, 1), new Wave("StrongEnemy", 2, 1) }, new float[] { 1 * 4, 1 * 5, 0 });
        rounds[7] = new Round(new Wave[] { new Wave("FastEnemy", 2, 2f), new Wave("BasicEnemy", 6, 1) }, new float[] { 2 * 2, 0 });
        rounds[8] = new Round(new Wave[] { new Wave("BasicEnemy", 18, .5f) }, new float[] { 0 });
        rounds[9] = new Round(new Wave[] { new Wave("StrongEnemy", 10, 1) }, new float[] { 0 });
        rounds[10] = new Round(new Wave[] { new Wave("ExplosionResistantEnemy", 5, .75f), new Wave("FastEnemy", 2, 1) }, new float[] { .75f * 5, 0 });
        rounds[11] = new Round(new Wave[] { new Wave("SlipperyEnemy", 15, 1.4f) }, new float[] { 0 });
        rounds[12] = new Round(new Wave[] { new Wave("BasicEnemy", 10, 1), new Wave("SlipperyEnemy", 10, 1.85f) }, new float[] { 1 * 10, 0 });
        rounds[13] = new Round(new Wave[] { new Wave("FastEnemy", 30, 1) }, new float[] { 0 });
        rounds[14] = new Round(new Wave[] { new Wave("StrongEnemy", 8, 1f), new Wave("ScoutEnemy", 5, 1) }, new float[] { 1 * 8, 0 });
        rounds[15] = new Round(new Wave[] { new Wave("StrongEnemy", 16, .35f) }, new float[] { 0 });
        rounds[16] = new Round(new Wave[] { new Wave("ExplosionResistantEnemy", 10, .8f), new Wave("SlipperyEnemy", 10, 1) }, new float[] { .8f * 10, 0 });
        rounds[17] = new Round(new Wave[] { new Wave("ArmoredEnemy", 20, 1.55f) }, new float[] { 0 });
        rounds[18] = new Round(new Wave[] { new Wave("FastEnemy", 24, .35f) }, new float[] { 0 });
        rounds[19] = new Round(new Wave[] { new Wave("ArmoredEnemy", 10, 1.5f), new Wave("SlipperyEnemy", 10, 1) }, new float[] { 1.5f * 10, 0 });

        //  Set up UI.
        roundText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        moneyText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        livesText = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
    }

    //  Run all the code
    private void Update()
    {
        UpdateText();
        ChangeMenu();
        PlaceTower();
        ActivateUpgradeButtons();
        if (Input.GetMouseButtonDown(0) && !overMenu) { selected = false; }
    }

    //  Check if mouse is over the menu panel.
    private void OnMouseEnter()
    {
        overMenu = true;
    }
    private void OnMouseExit()
    {
        overMenu = false;
    }
    #endregion
}