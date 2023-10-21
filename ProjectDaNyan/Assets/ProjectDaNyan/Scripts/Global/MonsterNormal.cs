using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;

public class MonsterNormal : MonoBehaviour
{
    public IObjectPool<GameObject> myPool { get; set; }

    [SerializeField] private MonsterStatus _monsterStatus;

    // nav mesh related variables 
    private NavMeshAgent _navMeshAgent = null;
    private Transform _target = null;

    [Header("Attack Range Setting")]

    [SerializeField] private GameObject _monsterBulletPrefab;

    // attack range
    [SerializeField] private float _attackPower;
    [SerializeField] private float _monsterHP;
    [SerializeField] private float _monsterSpeed;
    [SerializeField] private float _attackRange;
    [SerializeField] private float _attackinterval;
    [SerializeField] private float _bulletSpeed = 5f;
    [SerializeField] private state _currentState;

    public Transform _attackPoint;
    private float _attacktime;


    // Monster state
    public enum state
    {
        chasing,
        attack,
        dead
    }


    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        // set target 
        _target = FindAnyObjectByType<PlayerStatus>().transform;
    }


    private void FixedUpdate()
    {
        if(_currentState == state.attack)
        {
            this.transform.LookAt(_target);

        }
    }


    private void OnEnable()
    {
        _currentState = state.chasing;

        _navMeshAgent.stoppingDistance = _attackRange = _monsterStatus.attackRange;

        _attackPower = _monsterStatus.attackPower;
        _monsterHP = _monsterStatus.hp;
        _monsterSpeed = _monsterStatus.speed;
        _attackRange = _monsterStatus.attackRange;
        _attackinterval = _monsterStatus.attackSpeed;

        _navMeshAgent.speed = _monsterSpeed;
        _navMeshAgent.SetDestination(_target.position);

        GameManager.Inst.delegateGameState += PrepareBossStage;

        StartCoroutine(monsterState());
    }

    IEnumerator monsterState()
    {

        while (_monsterHP > 0)
        {
            yield return new WaitForSeconds(0.1f);

            checkAttackDistance();

            yield return StartCoroutine(_currentState.ToString());
        }

        StartCoroutine(dead());
    }

    IEnumerator chasing()
    {
        _navMeshAgent.SetDestination(_target.transform.position);
        yield return null;
    }

    IEnumerator attack()
    {
        _attacktime += 0.1f;


        if(_attacktime >= _attackinterval)
        {
            _attacktime = 0;
            attackPlayer();
        }
        yield return null;
    }

    IEnumerator dead()
    {
        GameManager.Inst.delegateGameState -= PrepareBossStage;

        ObjectPoolManager.Inst.DestroyObject(this.gameObject);
        yield return null;
    }

    private void checkAttackDistance()
    {

        float distance = Vector3.Distance(this.transform.position, _target.transform.position);

        if (distance >= 45)
        {
            _currentState = state.dead;
        }

        else if (_navMeshAgent.remainingDistance <= _attackRange && distance <= _attackRange)
        {
            _navMeshAgent.isStopped = true;
            _currentState = state.attack;
        }
        else
        {
            _navMeshAgent.isStopped = false;
            _currentState = state.chasing;
        }

        Debug.Log($"current state {_currentState}");
    }


    private void attackPlayer()
    {
        // attacking player
        GameObject bullet = ObjectPoolManager.Inst.BringObject(_monsterBulletPrefab);
        bullet.transform.position = _attackPoint.position;
        bullet.GetComponent<TempBullet>().Damage = _attackPower;
        bullet.transform.LookAt(_target);

        Vector3 bulletDir = _target.position - this.transform.position;

        Rigidbody bulletRB = bullet.GetComponent<Rigidbody>();
        bulletRB.velocity = bulletDir * _bulletSpeed;
    }


    public void PrepareBossStage(GameState gameState)
    {
        if(gameState == GameState.bossReady)
        {
            Debug.Log("monster, boss ready");
            ObjectPoolManager.Inst.DestroyObject(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"detect collision {other.gameObject}");


        if (other.tag.Equals("PlayerAttack"))
        {

            _monsterHP -= 1;

            //// get bullet damage 
            //if (other.gameObject.TryGetComponent(out Bullet bullet))
            //{
            //    // apply player attack damage 
         
            //}
        }
    }
}
