using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RepositoryBase : MonoBehaviour
{
    public abstract void Add(ScoreEventData item);
    public abstract IEnumerable<ScoreEventData> FindAll();

    public abstract void Load();
    public abstract void Save();
}
