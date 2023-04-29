using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public abstract class StateAction : ScriptableObject
{
    public abstract void OnAction(StateEventArguments args);

}
