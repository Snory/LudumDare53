using LootLocker.Requests;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderBoard : MonoBehaviour 
{

    public RepositoryBase ScoreRepository;

    // Start is called before the first frame update
    void Start()
    {
        ScoreRepository.Load();
    }

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    private void OnApplicationQuit()
    {
        ScoreRepository.Save();
    }

    public void OnHighScoreAdded(EventArgs highScoreData)
    {        
        ScoreRepository.Add((ScoreEventData) highScoreData);
    }
}
