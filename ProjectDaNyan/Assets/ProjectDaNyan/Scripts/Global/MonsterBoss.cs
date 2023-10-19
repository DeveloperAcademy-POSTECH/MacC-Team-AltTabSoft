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
    [SerializeField] private float _dashSpeed = 10;

    // boss monster current state 
    [SerializeField] private BossState _currentState;

    [SerializeField] private float _readyDashTime = 3;
    [SerializeField] private float _dashAttackTime = 5;
    [SerializeField] private float _normalAttackCount = 3;

    private float _dashReady;
    private float _dashTime;
    private float _attackTime;
    private float _normalAttackedCount = 0;


    private Rigidbody _monsterRigidbody;
    private Vector3 _dashDirection = Vector3.forward;

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


        if(TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            _monsterRigidbody = rb;
        }


        StartCoroutine(idle());
    }


    private void FixedUpdate()
    {
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

        switch (_currentState)
        {
            case BossState.chasing:
            case BossState.normalAttack:
                if (_navMeshAgent.isActiveAndEnabled == false)
                {
                    _navMeshAgent.enabled = true;
                }

                checkAttackDistance();
                StartCoroutine(_currentState.ToString());
                break;

            case BossState.readyDashAttack:
                StartCoroutine(readyDashAttack());
                break;

            case BossState.dashAttack:
                StartCoroutine(dashAttack());
                break;


            case BossState.wideAttack:
                StartCoroutine(wideAttack());
                break;

            case BossState.dead:
                StartCoroutine(dead());
                break;
        }
    }



    IEnumerator chasing()
    {
        _navMeshAgent.SetDestination(_target.transform.position);
        yield return null;

        StartCoroutine(idle());
    }

    IEnumerator normalAttack()
    {
        // normal attack animation
        //**** need to edit, animation required! **** 
        // apply damage to player

        _attackTime += 0.1f;

        if(_attackTime >= _attackSpeed)
        {
            _attackTime = 0;
            attackPlayer();
        }


        if (_normalAttackedCount <= _normalAttackCount)
        {
            _normalAttackedCount += 1;
            _currentState = BossState.readyDashAttack;

        }

        _normalAttackedCount = 0;

        yield return null;

        StartCoroutine(idle());
    }

    IEnumerator readyDashAttack()
    {
       if(_navMeshAgent.isActiveAndEnabled == true)
       {
            _navMeshAgent.enabled = false;
       }


        while (_dashReady >= _readyDashTime)
        {
            _dashReady += 1;

            yield return new WaitForSeconds(1);
        }

        _dashReady = 0f;
        _currentState = BossState.dashAttack;

        yield return null;

        StartCoroutine(idle());
    }

    IEnumerator dashAttack()
    {
        _dashTime += 1f;

        while (_dashTime <= _dashAttackTime)
        {
            yield return new WaitForSeconds(1);
        }

        _currentState = BossState.chasing;

        StartCoroutine(idle());
    }

    IEnumerator wideAttack()
    {
        Debug.Log("boss wide attack");

        _currentState = BossState.chasing;

        yield return null;

        StartCoroutine(idle());
    }

    IEnumerator dead()
    {
        GameManager.Inst.BossDead();
        yield return null;
    }


    // check distance between boss and player 
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


    private void OnTriggerEnter(Collider other)
    {

        if (other.tag.Equals("Wall"))
        {
            Vector3 incidenceVector = this.transform.forward;
            Vector3 normalVector = other.ClosestPoint(this.transform.position).normalized;
            //collision.contacts[0].normal;

            Debug.Log($"incidence Vector : {incidenceVector} // normal Vector : {normalVector}");


            _dashDirection = Vector3.Reflect(incidenceVector, normalVector);
        }

   

        if (other.tag.Equals("PlayerAttack"))
        {
            // get bullet damage 
            if (other.gameObject.TryGetComponent<Bullet>(out Bullet bullet))
            {
                int damage = bullet.damage;
            }

            // monster damaged
            _monsterHP -= 1;
        }

    }

}
