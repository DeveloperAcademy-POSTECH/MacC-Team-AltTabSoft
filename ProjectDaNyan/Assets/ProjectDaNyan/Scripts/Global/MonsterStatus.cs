using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterStatus", menuName = "ScriptableObjects/MonsterScriptableObject", order = 1)]
public class MonsterStatus : ScriptableObject
{
    // monster attack power 
    public float attackPower;

    // monster HP 
    public float hp;

    // monster move speed
    public float speed;

    // monster attack distance 
    public float attackRange;

    // monster attack speed 
    public float attackSpeed;
}
