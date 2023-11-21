using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterBossAData", menuName = "Scriptable_Object/MonsterBossAData")]
public class MonsterBossAData : ScriptableObject
{
    [Header("Boss Monster A Status")]
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

    [Space] [Header("Boss Skill Data")] 
    
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

    // big wave interval
    public float BigWaveInterval;

    // big wave count
    public float BigWaveCount;

}
