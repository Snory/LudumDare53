using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class LeaderboardItemUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _nameText;

    [SerializeField]
    private TextMeshProUGUI _scoreText;

    public void Initialize(string name, int score)
    {
        _nameText.text = name;
        _scoreText.text = score.ToString();
    }

}
