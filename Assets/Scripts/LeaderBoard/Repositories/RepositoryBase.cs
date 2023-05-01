using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RepositoryBase : MonoBehaviour
{
    public abstract void SetName(string name);
    public abstract void Add(ScoreEventData item);
    public abstract void FindAll(Action<List<ScoreEventData>> callback);

    public abstract void Load();
    public abstract void Save();
}
