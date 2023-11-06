using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(JoystickController),true)]
public class JoystickControllerEditor : FloatingJoystickEditor
{
    private SerializedProperty _playerState;
    private SerializedProperty _playerData;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        PlayerState _playerState;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        _playerState = serializedObject.FindProperty("_playerState");
    }

    protected override void DrawComponents()
    {
        base.DrawComponents();
        EditorGUILayout.ObjectField(_playerState, new GUIContent("PlayerState","PlayerStatus"));
    }
}
