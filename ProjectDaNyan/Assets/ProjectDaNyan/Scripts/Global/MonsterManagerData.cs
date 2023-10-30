using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterManagerData", menuName = "Scriptable_Object/MonsterManagerData")]
public class MonsterManagerData : ScriptableObject
{
    [Header("## Monster spawn interval ##")]
    public float SpawnInterval;
    [Space]

    [Header("## Total monster quantity at once ##")]
    public int TotalSpawnQty;
    [Space]

    [Header("## Normal Monster Short Range Spawn Quantity ##")]
    public int NormalShortRangeQty;
    [Space]

    [Header("## Normal Monster Long Range Spawn Quantity ##")]
    public int NormalLongRangeQty;
    [Space]

    [Header("## Elite Monster Short Range Spawn Quantity ##")]
    public int EliteShortRangeQty;
    [Space]

    [Header("## Elite Monster Long Range Spawn Quantity ##")]
    public int EliteLongRangeQty;
    [Space]

    [Header("## Select Boss Monster Type ##")]
    public BossType BossMonsterType;

}
