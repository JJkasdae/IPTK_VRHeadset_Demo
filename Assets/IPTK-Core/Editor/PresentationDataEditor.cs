using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PresentationData))]
public class PresentationDataEditor : Editor
{
    private SerializedProperty _name;
    private SerializedProperty _description;
    private SerializedProperty _sessions;
    private SerializedProperty _transitions;
    private SerializedProperty _timeline;
    private void OnEnable()
    {
        _name = serializedObject.FindProperty("_name");
        _description = serializedObject.FindProperty("_description");
        _sessions = serializedObject.FindProperty("_sessions");
        _transitions = serializedObject.FindProperty("_transitions");
        _timeline = serializedObject.FindProperty("_timeline");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        //base.OnInspectorGUI();
        //EditorGUILayout.LabelField(_name.stringValue.ToUpper(), EditorStyles.boldLabel);
        //EditorGUILayout.Space(5);

        EditorGUILayout.LabelField("Presentation General Stats", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(_name, new GUIContent("Name"));
        EditorGUILayout.PropertyField(_description, new GUIContent("Description"));

        EditorGUILayout.LabelField("Timelines", EditorStyles.boldLabel);
        //EditorGUILayout.PropertyField(_sessions, new GUIContent("Sessions"));
        //EditorGUILayout.PropertyField(_transitions, new GUIContent("Transitions"));
        EditorGUILayout.PropertyField(_timeline, new GUIContent("Timeline"));


        serializedObject.ApplyModifiedProperties();
    }
}
