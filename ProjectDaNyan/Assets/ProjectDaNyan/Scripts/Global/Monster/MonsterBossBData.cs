using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "BossTypeBData", menuName = "Scriptable_Object/BossTypeBData")]
public class MonsterBossBData : ScriptableObject
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

    // monster normal attack 
    public float AttackCount;
    
    // idle time
    public float IdleTime;

    [Space] [Header("Boss Skill Data")] 
    
    public float MachineGunRate;
    public float ReadyMachineGunTime;
    public float MachineGunCount;
    public float MachineGunSpeed;

    public float ReadyFatManTime;
    public float FatManRate;
    public float FatManCount;
    public float FatManDamage;
}
