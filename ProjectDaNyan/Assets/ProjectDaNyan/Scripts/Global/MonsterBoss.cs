using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;

public class MonsterBoss : MonoBehaviour
{
    public IObjectPool<GameObject> myPool { get; set; }

    [SerializeField] private MonsterStatus monsterStatus;

    // Boss Monster state
    private enum BossState
    {
        chasing,
        normalAttack,
        dashAttack,
        readyDashAttack,
        wideAttack,
        dead
    }

    // boss monstser status
    [SerializeField] private float _attackPower;
    [SerializeField] private float _monsterHP;
    [SerializeField] private float _monsterSpeed;
    [SerializeField] private float _attackRange;
    [SerializeField] private float _attackSpeed;
    [SerializeField] private float _dashSpeed = 100;

    // boss monster current state 
    [SerializeField] private BossState _currentState;

    [SerializeField] private float _readyDashTime = 3;
    [SerializeField] private float _dashTime = 5;
    [SerializeField] private float _normalAttackCount = 3;

    private float _normalAttackTime;
    private float _normalAttackedCount = 0;


    private Rigidbody _monsterRigidbody;
    private Vector3 _dashDirection;

    private NavMeshAgent _navMeshAgent = null;
    private GameObject _target = null;

    private void OnEnable()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        // set target 
        _target = FindAnyObjectByType<PlayerController>().gameObject;

        _currentState = BossState.chasing;

        _attackPower = monsterStatus.attackPower;
        _monsterHP = monsterStatus.hp;
        _monsterSpeed = monsterStatus.speed;
        _attackRange = monsterStatus.attackRange;
        _attackSpeed = monsterStatus.attackSpeed;


        //monsterRigidbody = GetComponent<Rigidbody>();

        StartCoroutine(idle());

    }


    private void FixedUpdate()
    {


        // if current state is not in dash attack nor ready dash attack, don't do anything 
        if (_currentState != BossState.dashAttack || _currentState != BossState.readyDashAttack)
            return;



        switch (_currentState)
        {
            // look at target 
            case BossState.readyDashAttack:

                this.transform.LookAt(_target.transform);

                break;

            // dash to target 
            case BossState.dashAttack:

                _monsterRigidbody.velocity = _dashDirection * _dashSpeed;

                break;

            // set velocity to zero 
            default:
                //monsterRigidbody.velocity = Vector3.zero;
                break;
        }

    }



    // chasing => normal attack * 3 => dash attack => wide attack => chasing 

    IEnumerator idle()
    {
        yield return new WaitForSeconds(0.1f);

        Debug.Log($"Boss monster current state : {_currentState}");

        while (_monsterHP > 0)
        {

            switch (_currentState)
            {
                case BossState.chasing:
                case BossState.normalAttack:
                    //checkAttackDistance();
                    break;
            }



            yield return StartCoroutine(_currentState.ToString());
        }
    }



    IEnumerator chasing()
    {
        _navMeshAgent.SetDestination(_target.transform.position);

        yield return null;
    }





    IEnumerator normalAttack()
    {
        // normal attack animation
        //**** need to edit, animation required! **** 
        // apply damage to player 
        attackPlayer();

        _normalAttackedCount += 1;

        if (_normalAttackedCount >= _normalAttackCount)
        {
            _currentState = BossState.readyDashAttack;

            _normalAttackedCount = 0;

            //StartCoroutine(idle());
            yield break;
        }
        yield return new WaitForSeconds(_attackSpeed);
    }


    IEnumerator readyDashAttack()
    {
        while (_readyDashTime > 0)
        {
            _readyDashTime -= 1;

            yield return new WaitForSeconds(1);
        }

        _currentState = BossState.dashAttack;

        yield return null;
    }


    IEnumerator dashAttack()
    {
        while (_dashTime > 0)
        {
            _dashTime -= 1;
            yield return new WaitForSeconds(1);
        }

        _currentState = BossState.chasing;
    }

    IEnumerator wideAttack()
    {

        yield return new WaitForSeconds(_attackSpeed);
    }

    IEnumerator dead()
    {

        yield return null;
    }



    private void checkAttackDistance()
    {
        float distance = Vector3.Distance(this.transform.position, _target.transform.position);


        if (_navMeshAgent.remainingDistance <= _attackRange && distance <= _attackRange)
        {
            _navMeshAgent.isStopped = true;
            _currentState = BossState.normalAttack;
        }
        else
        {
            _navMeshAgent.isStopped = false;
            _currentState = BossState.chasing;
        }
    }


    private void attackPlayer()
    {
        Debug.Log("Boss monster normal attack");

        _target.SendMessage("ApplyDamage", _attackPower, SendMessageOptions.DontRequireReceiver);
    }


    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.tag.Equals("PlayerAttack"))
        {
            // monster damaged
            _monsterHP -= 1;
        }



        // check wall during dash 
        if (_currentState == BossState.dashAttack)
        {
            if (collision.gameObject.tag.Equals("Wall"))
            {
                Vector3 incidenceVector = this.transform.forward;
                Vector3 normalVector = collision.contacts[0].normal;

                _dashDirection = Vector3.Reflect(incidenceVector, normalVector);
            }
        }

    }

}
