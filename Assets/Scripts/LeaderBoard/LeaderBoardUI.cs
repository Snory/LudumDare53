using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LeaderBoardUI : MonoBehaviour
{
    private LeaderBoard _leaderBoard;

    [SerializeField]
    private GameObject _scoreItemUI;

    [SerializeField]
    private Transform _scoreItemContentTransform;

    private void Awake()
    {
        _leaderBoard = FindObjectOfType<LeaderBoard>();

        if (_leaderBoard != null)
        {
            Initialize();
        }
    }

    private void Initialize()
    {
        _leaderBoard.ScoreRepository.FindAll(OnDataRetrieved);
    }

    private void OnDataRetrieved(List<ScoreEventData> scores)
    {
        foreach (var score in scores)
        {
            GameObject o = Instantiate(_scoreItemUI, _scoreItemContentTransform.position, Quaternion.identity, _scoreItemContentTransform);
            LeaderboardItemUI leaderboardItemUI = o.GetComponent<LeaderboardItemUI>();
            leaderboardItemUI.Initialize(score.ScoreData.Name, score.ScoreData.Score);
        }
    }


}
