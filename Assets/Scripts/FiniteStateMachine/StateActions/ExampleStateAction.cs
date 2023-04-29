using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewExampleStateAction", menuName = "FSM/Actions/ExampleStateAction", order = 1)]
public class ExampleStateAction : StateAction
{
    public override void OnAction(StateEventArguments args)
    {
        Debug.Log("On action");
    }
}
