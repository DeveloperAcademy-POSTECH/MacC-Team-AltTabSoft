using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class MonsterBoss : MonoBehaviour
{
    public IObjectPool<GameObject> myPool { get; set; }

    // Boss Monster state
    public enum BossState
    {
        Searching,
        attackDash,
        attackBomb,
        attackWide,
        dead
    }

    // boss monstser status
    [SerializeField] float attackRange;
    [SerializeField] float attackSpeed;
    [SerializeField] float damage;
    [SerializeField] float monsterHP;
    [SerializeField] float monsterDef;

    // boss monster current state 
    [SerializeField] protected BossState currentState;






}
