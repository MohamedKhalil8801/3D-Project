﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace GGSFPSIntegrationTool.Scripts.Editor
{
    [CustomEditor(typeof(NonPlayer.NonPlayerRespawn)), CanEditMultipleObjects]
    public class NonPlayerRespawnEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("_WillSpawnOnStart"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_NonPlayerCharacters"));

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Respawn Locations", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_RespawnPoints"));
            if (serializedObject.FindProperty("_RespawnPoints").arraySize == 0)
            {
                serializedObject.FindProperty("_RespawnPoints").arraySize = 1;
            }
            if (serializedObject.FindProperty("_RespawnPoints").arraySize > 1)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("_RespawnPointOrder"));
            }

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Positioning Offsets", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_WillSpawnOnGround"));
            if (serializedObject.FindProperty("_WillSpawnOnGround").boolValue)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("_PivotToBottomHeight"));
            }
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_RandomisedHorizontalRadiusOffset"));
            if (!serializedObject.FindProperty("_WillSpawnOnGround").boolValue)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("_RandomisedVerticalUpwardsOffset"));
            }
            EditorGUILayout.Slider(serializedObject.FindProperty("_RandomisedYRotationOffset"), 0f, 180f);

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Events", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_OnSpawn"));

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Automatic Respawning", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_WillAutoRespawn"));
            if (serializedObject.FindProperty("_WillAutoRespawn").boolValue)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("_AutoRespawnDelayInSeconds"));
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
