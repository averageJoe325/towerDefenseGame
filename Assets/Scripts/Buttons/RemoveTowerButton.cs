using UnityEngine;
using UnityEngine.UI;

//  Removes the current tower if the Menu Panel is clicked.
public class RemoveTowerButton : MonoBehaviour
{
    //  Set up listener.
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => GameManager.singleton.DestroyCurrentTower());
    }
}