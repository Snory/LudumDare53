using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewExampleState", menuName = "FSM/States/ExampleState", order = 1)]
public class ExampleState : State
{
    public override void OnEnter(StateEventArguments stateEventArguments)
    {
        Debug.Log("On enter");
    }

    public override void OnExit(StateEventArguments stateEventArguments)
    {
        Debug.Log("On exit");
    }
}
