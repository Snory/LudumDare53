using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GeneralEvent))]
public class GeneralEventRaiseEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        GeneralEvent globalEvent = (GeneralEvent) target;
        if (GUILayout.Button("Raise"))
        {
            globalEvent.Raise();
        }
    }

}
