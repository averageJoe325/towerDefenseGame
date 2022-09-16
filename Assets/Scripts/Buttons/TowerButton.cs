using TMPro;
using UnityEngine;
using UnityEngine.UI;

//  Spawns tower when the player clicks the button.
public class TowerButton : MonoBehaviour
{
    private string path;
    private int price;

    //  Figure out which tower to spawn and set up listener.
    private void Start()
    {
        path = "Prefabs/Towers/" + gameObject.name.Substring(0, gameObject.name.Length - 6);
        string text = transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;
        price = int.Parse(text.Substring(text.IndexOf('$') + 1, text.IndexOf(')') - text.IndexOf('$') - 1));
        GetComponent<Button>().onClick.AddListener(SpawnTower);
    }

    //  Spawn tower.
    private void SpawnTower()
    {
        GameManager.singleton.DestroyCurrentTower();
        GameManager.singleton.currentTower = (GameObject)Instantiate(Resources.Load(path), GameObject.Find("GamePanel").transform);
        if (GameManager.singleton.currentTower.GetComponent<Tower>().price > GameManager.singleton.money)
            GameManager.singleton.DestroyCurrentTower();
    }

    //  Disable the button if the price of the tower is too high.
    private void Update()
    {
        GetComponent<Button>().interactable = price <= GameManager.singleton.money;
    }
}