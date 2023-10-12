using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class MonsterBoss : MonoBehaviour
{
    public IObjectPool<GameObject> myPool { get; set; }

    public MonsterStatus monsterStatus;

    // Boss Monster state
    public enum BossState
    {
        Searching,
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

    // boss monster current state 
    [SerializeField] protected BossState currentState;




    private void OnEnable()
    {

        attackPower = monsterStatus.attackPower;
        monsterHP = monsterStatus.hp;
        monsterSpeed = monsterStatus.speed;
        attackRange = monsterStatus.attackRange;
        attackSpeed = monsterStatus.attackSpeed;



    }


    IEnumerator bossState()
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

    IEnumerator Searching()
    {


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

        yield return new WaitForSeconds(attackSpeed);
    }

    IEnumerator wideAttack()
    {

        yield return new WaitForSeconds(attackSpeed);
    }

    IEnumerator dead()
    {

        yield return null;
    }

}

