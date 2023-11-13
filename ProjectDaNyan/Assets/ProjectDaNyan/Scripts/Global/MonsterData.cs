using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterData", menuName = "Scriptable_Object/MonsterData")]
public class MonsterData : ScriptableObject
{
    // monster type 
    public MonsterType monsterType;
    
    // monster attack power 
    public float attackPower;

    // monster HP 
    public float hp;

    // monster move speed
    public float speed;

    // monster attack distance 
    public float attackRange;

    // monster attack interval 
    public float attackInterval;

    // monster attack speed;
    public float attackSpeed;

    // monster exp
    public float exp;
}
