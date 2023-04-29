using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "NewGeneralTransition", menuName = "FSM/Transitions/GeneralTransition", order = 1)]

public class StateTransition : ScriptableObject
{
    public StateTransitionCondition TransitionCondition;
    public State StateToTransit;



}
