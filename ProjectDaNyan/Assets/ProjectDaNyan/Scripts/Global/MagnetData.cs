using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MagnetData", menuName = "Scriptable_Object/MagnetData")]

public class MagnetData : ScriptableObject
{
    public float magnetItemMoveSpeed = 10f;
    public float magnetDistance = 3f;
}
