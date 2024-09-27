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
        _Effect = serializedObject.FindProperty("_Effect");
        _importEffectAnimation = serializedObject.FindProperty("_importEffectAnimation");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.LabelField("Transition General Stats", EditorStyles.boldLabel);
        EditorGUILayout.ObjectField(_lastSession, new GUIContent("Last Session"));
        EditorGUILayout.ObjectField(_nextSession, new GUIContent("Next Session"));
        EditorGUILayout.PropertyField(_effect, new GUIContent("Effect"));
        EditorGUILayout.PropertyField(_Effect, new GUIContent("Effect"));

        if (_Effect.boolValue == true)
        {
            EditorGUILayout.PropertyField(_importEffectAnimation, new GUIContent("Transition Animation"));
        }

        serializedObject.ApplyModifiedProperties();
    }
}
