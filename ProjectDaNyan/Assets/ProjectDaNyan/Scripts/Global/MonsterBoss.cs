using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;

public class MonsterBoss : MonoBehaviour
{
    public IObjectPool<GameObject> myPool { get; set; }

    public MonsterStatus monsterStatus;

    // Boss Monster state
    public enum BossState
    {
        chasing,
        normalAttack,
        dashAttack,
        readyDashAttack,
        wideAttack,
        dead
    }

    // boss monstser status
    [SerializeField] float attackPower;
    [SerializeField] float monsterHP;
    [SerializeField] float monsterSpeed;
    [SerializeField] float attackRange;
    [SerializeField] float attackSpeed;
    [SerializeField] float dashSpeed = 100;

    // boss monster current state 
    [SerializeField] protected BossState currentState;

    [SerializeField] int readyDashTime = 3;
    [SerializeField] int dashTime = 5;
    [SerializeField] int normalAttackCount = 3;
    int normalAttackedCount = 0;


    Rigidbody monsterRigidbody;
    Vector3 dashDirection;

    NavMeshAgent myNavMeshAgent = null;
    GameObject target = null;

    private void OnEnable()
    {
        myNavMeshAgent = GetComponent<NavMeshAgent>();
        // set target 
        target = FindAnyObjectByType<PlayerController>().gameObject;

        currentState = BossState.chasing;

        attackPower = monsterStatus.attackPower;
        monsterHP = monsterStatus.hp;
        monsterSpeed = monsterStatus.speed;
        attackRange = monsterStatus.attackRange;
        attackSpeed = monsterStatus.attackSpeed;


        //monsterRigidbody = GetComponent<Rigidbody>();

        StartCoroutine(idle());

    }


    //private void FixedUpdate()
    //{


    //    // if current state is not in dash attack nor ready dash attack, don't do anything 
    //    if (currentState != BossState.dashAttack || currentState != BossState.readyDashAttack)
    //        return;



    //    switch (currentState)
    //    {
    //        // look at target 
    //        case BossState.readyDashAttack:

    //            this.transform.LookAt(target.transform);

    //            break;

    //        // dash to target 
    //        case BossState.dashAttack:

    //            monsterRigidbody.velocity = dashDirection * dashSpeed;

    //            break;

    //        // set velocity to zero 
    //        default:
    //            //monsterRigidbody.velocity = Vector3.zero;
    //            break;
    //    }

    //}



    // chasing => normal attack * 3 => dash attack => wide attack => chasing 

    IEnumerator idle()
    {
        yield return new WaitForSeconds(0.1f);

        Debug.Log($"Boss monster current state : {currentState}");

        while (monsterHP > 0)
        {

            switch (currentState)
            {
                case BossState.chasing:
                case BossState.normalAttack:
                    //checkAttackDistance();
                    break;
            }



            yield return StartCoroutine(currentState.ToString());
        }
    }



    IEnumerator chasing()
    {
        myNavMeshAgent.SetDestination(target.transform.position);

        yield return null;
    }





    IEnumerator normalAttack()
    {
        // normal attack animation
        //**** need to edit, animation required! **** 
        // apply damage to player 
        NormalAttack();

        normalAttackedCount += 1;

        if (normalAttackedCount >= normalAttackCount)
        {
            currentState = BossState.readyDashAttack;

            normalAttackedCount = 0;

            //StartCoroutine(idle());
            yield break;
        }
        yield return new WaitForSeconds(attackSpeed);
    }


    IEnumerator readyDashAttack()
    {
        while (readyDashTime > 0)
        {
            readyDashTime -= 1;

            yield return new WaitForSeconds(1);
        }

        currentState = BossState.dashAttack;

        yield return null;
    }


    IEnumerator dashAttack()
    {
        while (dashTime > 0)
        {
            dashTime -= 1;
            yield return new WaitForSeconds(1);
        }

        currentState = BossState.chasing;
    }

    IEnumerator wideAttack()
    {

        yield return new WaitForSeconds(attackSpeed);
    }

    IEnumerator dead()
    {

        yield return null;
    }



    void checkAttackDistance()
    {
        float distance = Vector3.Distance(this.transform.position, target.transform.position);


        if (myNavMeshAgent.remainingDistance <= attackRange && distance <= attackRange)
        {
            myNavMeshAgent.isStopped = true;
            currentState = BossState.normalAttack;
        }
        else
        {
            myNavMeshAgent.isStopped = false;
            currentState = BossState.chasing;
        }
    }


    public void NormalAttack()
    {
        Debug.Log("Boss monster normal attack");

        //target.SendMessage("ApplyDamage", attackPower, SendMessageOptions.DontRequireReceiver);
    }


    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.tag.Equals("PlayerAttack"))
        {
            // monster damaged
            monsterHP -= 1;
        }



        // check wall during dash 
        if (currentState == BossState.dashAttack)
        {
            if (collision.gameObject.tag.Equals("Wall"))
            {
                Vector3 incidenceVector = this.transform.forward;
                Vector3 normalVector = collision.contacts[0].normal;

                dashDirection = Vector3.Reflect(incidenceVector, normalVector);
            }
        }

    }

}
