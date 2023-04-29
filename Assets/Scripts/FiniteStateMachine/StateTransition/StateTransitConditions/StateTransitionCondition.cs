using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public abstract class StateTransitionCondition : ScriptableObject
{
    public abstract bool CanTransit(StateEventArguments transitionEventArguments);
}
