﻿
using Entities.Navigation;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomEditor(typeof(OwnerNavAgentManager))]
public class OwnerNavAgentInspector : Editor
{
    private OwnerNavAgentManager owner;
    private int id;
    private void OnEnable()
    {
        owner = target as OwnerNavAgentManager;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (Application.isPlaying)
        {
            id = EditorGUILayout.DelayedIntField(id);
            if (GUILayout.Button("Active Nav Id"))
            {
                owner.ActiveNav(id);
            }
        }
    }
}
#endif