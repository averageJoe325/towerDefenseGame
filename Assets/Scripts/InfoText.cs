using System;
using TMPro;
using UnityEngine;

//  Manages the text that appears after some rounds
public class InfoText : MonoBehaviour
{
    public static InfoText singleton;
    private string[] strings = new string[40];
    private SpriteRenderer sprite;
    private TextMeshProUGUI text;

    //  Set up variables.
    private void Start()
    {
        //  Set up singleton.
        if (singleton != null && singleton != this)
            Destroy(gameObject);
        singleton = this;

        //  Assign text.
        for (int i = 0; i < strings.Length; i++)
        {
            string s;
            switch (i)
            {
                case 0:
                    s = "Hello, and welcome to my game! Buy towers from the right hand of the screen, and when ready, hit play!";
                    break;
                case 1:
                    s = "First round complete! Click on towers to see their availible upgrades. Each tower has two.";
                    break;
                case 2:
                    s = "Warning: Not all enemies are created equal.";
                    break;
                case 5:
                    s = "Those bigger enemies are tougher than most others.";
                    break;
                case 7:
                    s = "Think fast!";
                    break;
                case 9:
                    s = "I hope you haven't been relying on one tower type...";
                    break;
                case 12:
                    s = "The dark enemies are immune to explosions, and the cyan enemies are slippery and can not be grabbed by claws.";
                    break;
                case 15:
                    s = "Those enemies that just ran away? Those were scouts! Their intel will be delivered and put into use in a few rounds.";
                    break;
                case 18:
                    s = "Those orange health bars represent armor. It must be broken before the enemy can be damaged, but some towers can deal extra damage to it.";
                    break;
                case 20:
                    s = "Remember those scouts? Well, it seems their info has just arrived. Get ready!";
                    break;
                default:
                    s = "";
                    break;
            }
            strings[i] = s;
        }

        //  Find the text mesh component.
        sprite = GetComponent<SpriteRenderer>();
        text = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        ChangeTextState(true);
    }

    //  Display the text correctly or remove the text.
    public void ChangeTextState(bool state)
    {
        int round = 0;
        try { round = GameManager.singleton.round; }
        catch { }
        string s = strings[round];
        if (s == "") { state = false; }
        sprite.enabled = state;
        if (state) { text.text = s; }
        text.enabled = state;
    }
}