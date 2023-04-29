using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewExampleTransitioNCondition", menuName = "FSM/Transitions/Conditions/ExampleTransitionCondition", order = 1)]
public class ExampleTransitionCondition : StateTransitionCondition
{
    public override bool CanTransit(StateEventArguments transitionEventArguments)
    {
        if (transitionEventArguments.GetType() != typeof(ExampleStateEventArgument)) return false;

        return true;
    }
}
