using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TimelineData))]
public class TimelineDataEditor : Editor
{
    private SerializedProperty _transitionData;

    private void OnEnable()
    {
        _transitionData = serializedObject.FindProperty("_transitionData");
    }

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI()
        serializedObject.Update();
        EditorGUILayout.LabelField("List of Transitions", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(_transitionData, new GUIContent("Transitions"));

        serializedObject.ApplyModifiedProperties();
    }
}
