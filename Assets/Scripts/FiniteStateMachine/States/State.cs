using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public abstract class State : ScriptableObject
{
    //possible transition for the state
    public List<StateTransition> Transitions;
    public List<StateAction> StateActions;

    public abstract void OnExit(StateEventArguments stateEventArguments);

    public abstract void OnEnter(StateEventArguments stateEventArguments);

    public virtual void OnUpdate(StateEventArguments stateEventArguments)
    {
        foreach(var action in StateActions)
        {
            action.OnAction(stateEventArguments);
        }
    }
    
    public State GetStateTransitTo(StateEventArguments eventArgs)
    {

        foreach (var transition in Transitions)
        {
            bool canTransit = transition.TransitionCondition.CanTransit(eventArgs);

            if (canTransit)
            {
                return transition.StateToTransit;
            }
        }
        return null;
    }
}
