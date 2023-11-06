using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(JoystickController),true)]
public class JoystickControllerEditor : FloatingJoystickEditor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void DrawComponents()
    {
        base.DrawComponents();
    }
}
