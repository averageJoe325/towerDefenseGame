using UnityEngine;
using UnityEngine.UI;

//  Defines Tower behavior.
public abstract class Tower : MonoBehaviour
{
    #region Variables
    public int price { get; protected set; }
    public bool[] upgrades { get; protected set; } = new bool[2];
    public string[] upgradesText { get; protected set; } = new string[2];
    public int[] upgradesPrice { get; protected set; } = new int[2];
    private bool isPlaced;
    private int inCol = -1;

    public float range { get; protected set; }
    protected float fireRate;
    protected string projectile;
    protected float projectileSpeed;
    protected float fireTimer;
    private GameObject target;
    #endregion


    #region Unity methods
    //  Make selected tower when clicked.
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(SelectTower);
    }

    //  Handle placing and firing of towers.
    private void Update()
    {
        TowerRangeVisible();
        RangeCircle();
        if (!isPlaced)
            Move();
        else
        {
            fireTimer += Time.deltaTime;
            target = Target();
            if (target != null && fireTimer > fireRate)
                Shoot();
        }
    }
    #endregion


    #region Buying and upgrading
    //  Move the tower to the desired location.
    private void Move()
    {
        GetComponent<RectTransform>().localPosition = Input.mousePosition - new Vector3(675, 540);
    }

    //  Keep track of the number of collision hitboxes the tower is in so it isn't placed improperly.
    private void OnTriggerEnter2D(Collider2D col)
    {
        inCol++;
    }
    private void OnTriggerExit2D(Collider2D col)
    {
        inCol--;
    }

    //  Make sure the tower is placeable at the current location and place it if able, return if placed.
    public bool Place()
    {
        if (inCol == 0)
        {
            isPlaced = true;
            GameManager.singleton.currentTower = null;
            GetComponent<Button>().enabled = true;
            GetComponent<Image>().enabled = true;
            return true;
        }
        return false;
    }

    //  Changes if the tower range is visible.
    private void TowerRangeVisible()
    {
        transform.GetChild(0).GetComponent<SpriteRenderer>().enabled
            = GameManager.singleton.currentTower == gameObject
            || (GameManager.singleton.selectedTower == gameObject && GameManager.singleton.selected);
    }

    //  Make the range circle accurate.
    private void RangeCircle()
    {
        transform.GetChild(0).GetComponent<RectTransform>().localScale = new Vector2(1, 1) * range * 2 / GetComponent<RectTransform>().localScale.x;
    }

    //  Select Tower
    private void SelectTower()
    {
        GameManager.singleton.selectedTower = gameObject;
        GameManager.singleton.selected = true;
    }

    //  Upgrade
    public virtual void Upgrade(int path)
    {
        upgrades[path] = true;
    }
    #endregion


    #region Attacking
    //  Find a target, if any, to shoot at.
    private GameObject Target()
    {
        GameObject targ = null;
        Enemy[] enemyList = FindObjectsOfType<Enemy>();
        GameObject[] gameObjects = new GameObject[enemyList.GetLength(0)];
        for (int i = 0; i < enemyList.GetLength(0); i++)
        {
            gameObjects[i] = enemyList[i].gameObject;
            if (enemyList[i].targetable
                && Vector2.Distance(gameObjects[i].GetComponent<RectTransform>().localPosition, GetComponent<RectTransform>().localPosition) < range
                && (targ == null || enemyList[i].DistanceToTravel() < targ.GetComponent<Enemy>().DistanceToTravel()))
                targ = gameObjects[i];
        }
        return targ;
    }

    //  Shoot at the target by spawning a projectile.
    protected virtual void Shoot()
    {
        fireTimer = 0;
        GameObject go = (GameObject)Instantiate(Resources.Load($"Prefabs/{projectile}"), GameObject.Find("GamePanel").transform);
        go.GetComponent<RectTransform>().localPosition = transform.localPosition;
        go.GetComponent<Projectile>().velocity
            = projectileSpeed * (target.GetComponent<RectTransform>().localPosition - GetComponent<RectTransform>().localPosition).normalized;
    }
    #endregion
}