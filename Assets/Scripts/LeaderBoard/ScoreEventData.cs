using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreEventData : EventArgs { 
    
    public ScoreData ScoreData;

    public ScoreEventData(ScoreData scoreData)
    {
        ScoreData = scoreData;
    }
}
