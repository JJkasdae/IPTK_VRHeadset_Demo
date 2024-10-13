using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TransitionData))]
public class TransitionDataEditor : Editor
{
    private SerializedProperty _lastSession;
    private SerializedProperty _nextSession;
    private SerializedProperty _effect;
    private SerializedProperty _Effect;
    private SerializedProperty _importEffectAnimation;

    private void OnEnable()
    {
        _lastSession = serializedObject.FindProperty("_lastSession");
        _nextSession = serializedObject.FindProperty("_nextSession");
        _effect = serializedObject.FindProperty("_effect");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.LabelField("Transition General Stats", EditorStyles.boldLabel);
        EditorGUILayout.ObjectField(_lastSession, new GUIContent("Last Session"));
        EditorGUILayout.ObjectField(_nextSession, new GUIContent("Next Session"));
        EditorGUILayout.PropertyField(_effect, new GUIContent("Effect"));

        serializedObject.ApplyModifiedProperties();
    }
}
