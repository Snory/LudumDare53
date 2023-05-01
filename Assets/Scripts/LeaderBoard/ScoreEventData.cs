using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreEventData : EventArgs { 
    
    public ScoreData ScoreData;
    public Action SavedCallBack;

    public ScoreEventData(ScoreData scoreData, Action savedCallback = null)
    {
        ScoreData = scoreData;
        SavedCallBack = savedCallback;
    }
}
