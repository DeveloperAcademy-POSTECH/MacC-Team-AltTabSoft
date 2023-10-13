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
        wideAttack,
        dead
    }

    // boss monstser status
    [SerializeField] float attackPower;
    [SerializeField] float monsterHP;
    [SerializeField] float monsterSpeed;
    [SerializeField] float attackRange;
    [SerializeField] float attackSpeed;
    [SerializeField] float dashSpeed = 10;

    // boss monster current state 
    [SerializeField] protected BossState currentState;

    [SerializeField] int dashTime = 5;

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


        monsterRigidbody = GetComponent<Rigidbody>();

        StartCoroutine(idle());

    }


    IEnumerator idle()
    {

        while (monsterHP > 0)
        {
            yield return StartCoroutine(currentState.ToString());
        }
    }



    //normalAttack,
    //dashAttack,
    //wideAttack,
    //dead

    IEnumerator chasing()
    {
        myNavMeshAgent.SetDestination(target.transform.position);

        yield return null;
    }





    IEnumerator normalAttack()
    {
        // normal attack animation



        // apply damage to player 


        yield return new WaitForSeconds(attackSpeed);
    }


    IEnumerator dashAttack()
    {
        while(dashTime > 0)
        {
            monsterRigidbody.velocity = dashDirection * dashSpeed;


            dashTime -= 1;
            yield return new WaitForSeconds(1);
        }

        yield return null;
    }

    IEnumerator wideAttack()
    {

        yield return new WaitForSeconds(attackSpeed);
    }

    IEnumerator dead()
    {

        yield return null;
    }


    private void OnCollisionEnter(Collision collision)
    {

        // check wall during dash 
        if(currentState == BossState.dashAttack)
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
