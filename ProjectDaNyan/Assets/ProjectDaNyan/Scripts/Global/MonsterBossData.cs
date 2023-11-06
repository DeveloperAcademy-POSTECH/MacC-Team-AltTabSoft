using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterBossData", menuName = "Scriptable_Object/MonsterBossData")]
public class MonsterBossData : ScriptableObject
{
    // monster attack power 
    public float AttackPower;

    // monster HP 
    public float HP;

    // monster move speed
    public float Speed;

    // monster attack distance 
    public float AttackRange;

    // monster attack interval 
    public float AttackInterval;

    // monster attack speed;
    public float AttackSpeed;

    // monster exp
    public float AttackCount;

    // time delay before dash attack
    public float ReadyDashTime;

    // dash time
    public float DashPoint;

    // dash speed
    public float DashSpeed;

    // dash count
    public float DashCount;

    // idle time
    public float IdleTime;
}
