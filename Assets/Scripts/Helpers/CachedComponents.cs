using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CachedComponents : MonoBehaviour
{
    private Dictionary<Type, Component> _cachedComponents;

    private void Awake()
    {
        _cachedComponents = new Dictionary<Type, Component>(); 
    }

    public new T GetComponent<T> () where T : Component
    {
        if(_cachedComponents.ContainsKey(typeof(T)))
        {
            return _cachedComponents[typeof(T)] as T;
        }

        var component = this.transform.root.GetComponentInChildren<T>();
        if(component != null)
        {
            _cachedComponents.Add(typeof(T), component);
        } else
        {
            Debug.LogError("[" + this.gameObject.name + "] cant find component of type: " + typeof(T).ToString());
        }
        return component;
    }
}
