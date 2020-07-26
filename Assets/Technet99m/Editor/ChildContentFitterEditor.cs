using UnityEngine;
using UnityEditor;
using System;

namespace Technet99m
{
    [CustomEditor(typeof(ChildContentFitter))]
    public class ChildContentFitterEditor : Editor
    {
        override public void OnInspectorGUI()
        {
            serializedObject.Update();
            var myScript = target as ChildContentFitter;
            EditorGUILayout.PropertyField(serializedObject.FindProperty("target"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("Horizontal"));
            if (myScript.Horizontal == FitMode.WithOffsets)
            {
                myScript.left = EditorGUILayout.FloatField("Left Border", myScript.left);
                myScript.right = EditorGUILayout.FloatField("Right Border", myScript.right);
                myScript.minWidth = EditorGUILayout.FloatField("Min Width", myScript.minWidth);
            }
            EditorGUILayout.PropertyField(serializedObject.FindProperty("Vertical"));
            if (myScript.Vertical == FitMode.WithOffsets)
            {
                myScript.top = EditorGUILayout.FloatField("Top Border", myScript.top);
                myScript.bottom = EditorGUILayout.FloatField("Bottom Border", myScript.bottom);
                myScript.minHeight = EditorGUILayout.FloatField("Min Height", myScript.minHeight);
            }
            serializedObject.ApplyModifiedProperties();
            if (serializedObject.FindProperty("target").objectReferenceValue != null)
                myScript.Fit();
        }
    }

}
