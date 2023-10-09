using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MonsterStatus : ScriptableObject
{
    // monster attack distance 
    public float attackRange;

    // monster attack speed 
    public float attackSpeed;

    // monster damage
    public float damage;

    // monster HP 
    public float hp;

    // monster defense
    public float def;
}
