using System;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{

    private static T _instance;
    public static T Instance { get => _instance; }

    private void Awake()
    {
        if(_instance != null)
        {
            throw new Exception($"[Singleton]: Trying to instantiate {this.gameObject.name} again.");
        }

        _instance = (T) this;

    }

    protected virtual void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
    }

}
