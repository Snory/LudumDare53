using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public struct ScoreData 
{
    public int Score;
    public string Name;

    public ScoreData(int score, string name)
    {
        Score = score;
        Name = name;
    }

}
