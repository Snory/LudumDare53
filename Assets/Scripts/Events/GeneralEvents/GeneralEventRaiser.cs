using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralEventRaiser : MonoBehaviour
{
    public GeneralEvent GeneralEventToRaise;

    public void Raise()
    {
        GeneralEventToRaise.Raise();
    }

}
