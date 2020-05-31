using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(AnimalAnimationController))]
public class AnimalAnimationControllerEditor : Editor
{
    bool fold;
    public override void OnInspectorGUI()
    {
        EditorGUILayout.PropertyField(serializedObject.FindProperty("sprites"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("verticalThreshhold"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("foods"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("foodSprite"));
        fold = EditorGUILayout.BeginFoldoutHeaderGroup(fold,"spriteRenderers");
        if(fold)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("body"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("face"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("eyes"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("right_leg"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("left_leg"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("right_hand"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("left_hand"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("tail"));
        }
        EditorGUILayout.PropertyField(serializedObject.FindProperty("normal"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("highlighted"));
        if (GUILayout.Button("Apply"))
            (target as AnimalAnimationController).Apply();
        serializedObject.ApplyModifiedProperties();
    }
}
