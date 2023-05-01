using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AddName : MonoBehaviour
{
    private LeaderBoard _leaderBoard;


    [SerializeField]
    private TMP_InputField _textInput;

    private void Awake()
    {
        _leaderBoard = FindObjectOfType<LeaderBoard>();

        if (PlayerPrefs.HasKey("PlayerName"))
        {
            _textInput.text = PlayerPrefs.GetString("PlayerName");
        }
    }

    public void OnNameChanged(string text)
    {
        if(_leaderBoard != null)
        {
            _leaderBoard.SetName(text);
        }

        _textInput.interactable = false;
    }
}
