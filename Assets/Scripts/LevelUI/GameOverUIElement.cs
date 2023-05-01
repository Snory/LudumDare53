using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverUIElement : MonoBehaviour
{

    [SerializeField]
    private TextMeshProUGUI _wonText;

    public void OnWon(EventArgs args)
    {
        GameOverEventArgs wonGameEventArgs = args as GameOverEventArgs;

        _wonText.text = "Your score is: " + wonGameEventArgs.Score.ToString();
    }
}
