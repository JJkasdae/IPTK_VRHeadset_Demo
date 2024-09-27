using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(SessionData))]
public class SessionDataEditor : Editor
{
    private SerializedProperty _name;
    private SerializedProperty _description;
    private SerializedProperty _sceneName;

    private void OnEnable()
    {
        _name = serializedObject.FindProperty("_name");
        _description = serializedObject.FindProperty("_description");
        _sceneName = serializedObject.FindProperty("_sceneName");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        //base.OnInspectorGUI();
        //EditorGUILayout.LabelField(_name.stringValue.ToUpper(), EditorStyles.boldLabel);
        //EditorGUILayout.Space(5);

        EditorGUILayout.LabelField("Session General Stats", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(_name, new GUIContent("Name"));
        EditorGUILayout.PropertyField(_description, new GUIContent("Description"));
        EditorGUILayout.PropertyField(_sceneName, new GUIContent("Scene Name"));

        serializedObject.ApplyModifiedProperties();
    }
}
