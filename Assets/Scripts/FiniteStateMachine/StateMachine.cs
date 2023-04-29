using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    [SerializeField]
    private CachedComponents _cachedComponents;
    public List<State> States;

    [SerializeField]
    private State _initState;

    private State _currentState;
    private StateEventArguments _currentStateEventArgs;

    private void Awake()
    {
        _currentState = _initState;
        _currentStateEventArgs = new StateEventArguments { CachedComponents = _cachedComponents};
    }
    
       private void Start()
    {
        _currentState.OnEnter(_currentStateEventArgs);
    }
 

    private void Update()
    {
        _currentState.OnUpdate(_currentStateEventArgs);
    }

    public void TransitTo(StateEventArguments args)
    {
        args.CachedComponents = _cachedComponents;
        
        State newState = _currentState.GetStateTransitTo(args);

        if(newState != null)
        {
            _currentState.OnExit(_currentStateEventArgs);
            _currentStateEventArgs = args;
            _currentState = newState;
            _currentState.OnEnter(_currentStateEventArgs); 
        }
    }



}
