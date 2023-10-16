using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;

public class MonsterNormal : MonoBehaviour
{
    public IObjectPool<GameObject> myPool { get; set; }

    public MonsterStatus monsterStatus;

    // nav mesh related variables 
    NavMeshAgent myNavMeshAgent = null;
    GameObject target = null;

    [Header("Attack Range Setting")]

    // attack range
    [SerializeField] float attackPower;
    [SerializeField] float monsterHP;
    [SerializeField] float monsterSpeed;
    [SerializeField] float attackRange;
    [SerializeField] float attackSpeed;


    // Monster state
    public enum state
    {
        chasing,
        attack,
        dead
    }

    [SerializeField] protected state currentState;


    void OnEnable()
    {
        myNavMeshAgent = GetComponent<NavMeshAgent>();

        // set target 
        target = FindAnyObjectByType<PlayerController>().gameObject;
        currentState = state.chasing;

        myNavMeshAgent.stoppingDistance = attackRange = monsterStatus.attackRange;

        attackPower = monsterStatus.attackPower;
        monsterHP = monsterStatus.hp;
        monsterSpeed = monsterStatus.speed;
        attackRange = monsterStatus.attackRange;
        attackSpeed = monsterStatus.attackSpeed;

        myNavMeshAgent.speed = monsterSpeed;
        StartCoroutine(monsterState());
    }

    IEnumerator monsterState()
    {
        while (monsterHP > 0)
        {
            checkAttackDistance();

            yield return StartCoroutine(currentState.ToString());
        }

        StartCoroutine(dead());
    }

    IEnumerator chasing()
    {
        myNavMeshAgent.SetDestination(target.transform.position);
        yield return null;
    }

    IEnumerator attack()
    {
        attackPlayer();

        yield return new WaitForSeconds(attackSpeed);
    }

    IEnumerator dead()
    {
        ObjectPoolManager.Inst.DestroyObject(this.gameObject);
        yield return null;
    }

    void checkAttackDistance()
    {

        float distance = Vector3.Distance(this.transform.position, target.transform.position);

        if (distance >= 45)
        {
            currentState = state.dead;
        }

        else if (myNavMeshAgent.remainingDistance <= attackRange && distance <= attackRange)
        {
            myNavMeshAgent.isStopped = true;
            currentState = state.attack;
        }
        else
        {
            myNavMeshAgent.isStopped = false;
            currentState = state.chasing;
        }
    }


    void attackPlayer()
    {
        // attacking player

        target.SendMessage("applyDamage", attackPower, SendMessageOptions.DontRequireReceiver);
    }



    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag.Equals("PlayerAttack"))
        {
            monsterHP = 0;
        }
    }
}
