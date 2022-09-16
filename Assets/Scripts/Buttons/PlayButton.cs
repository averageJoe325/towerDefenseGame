using UnityEngine;
using UnityEngine.UI;

//  Spawns tower when the player clicks the button.
public class PlayButton : MonoBehaviour
{
    //  Set up listener.
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(SpawnRound);
    }

    //  Spawn round.
    private void SpawnRound()
    {
        StartCoroutine(GameManager.singleton.SpawnRound());
    }

    //  Deactivate button when necessary.
    private void Update()
    {
        GetComponent<Button>().interactable = GameManager.singleton.IsRoundOver();
    }
}