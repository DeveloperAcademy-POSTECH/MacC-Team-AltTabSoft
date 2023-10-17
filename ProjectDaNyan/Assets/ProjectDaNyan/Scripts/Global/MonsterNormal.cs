using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;

public class MonsterNormal : MonoBehaviour
{
    public IObjectPool<GameObject> myPool { get; set; }

    [SerializeField]  MonsterStatus _monsterStatus;

    // nav mesh related variables 
    private NavMeshAgent _navMeshAgent = null;
    private GameObject _target = null;

    [Header("Attack Range Setting")]

    // attack range
    [SerializeField] private float _attackPower;
    [SerializeField] private float _monsterHP;
    [SerializeField] private float _monsterSpeed;
    [SerializeField] private float _attackRange;
    [SerializeField] private float _attackSpeed;
    private float _attacktime;


    // Monster state
    public enum state
    {
        chasing,
        attack,
        dead
    }

    public state CurrentState;


    private void OnEnable()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();

        // set target 
        _target = FindAnyObjectByType<PlayerController>().gameObject;
        CurrentState = state.chasing;

        _navMeshAgent.stoppingDistance = _attackRange = _monsterStatus.attackRange;

        _attackPower = _monsterStatus.attackPower;
        _monsterHP = _monsterStatus.hp;
        _monsterSpeed = _monsterStatus.speed;
        _attackRange = _monsterStatus.attackRange;
        _attackSpeed = _monsterStatus.attackSpeed;

        _navMeshAgent.speed = _monsterSpeed;

        StartCoroutine(monsterState());
    }

    IEnumerator monsterState()
    {
        checkAttackDistance();

        yield return new WaitForSeconds(0.1f);


        while (_monsterHP > 0)
        {
            yield return StartCoroutine(CurrentState.ToString());
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


        if(_attacktime >= _attackSpeed)
        {
            _attacktime = 0;
            attackPlayer();
        }
        yield return null;
    }

    IEnumerator dead()
    {
        ObjectPoolManager.Inst.DestroyObject(this.gameObject);
        yield return null;
    }

    private void checkAttackDistance()
    {

        float distance = Vector3.Distance(this.transform.position, _target.transform.position);

        if (distance >= 45)
        {
            CurrentState = state.dead;
        }

        else if (_navMeshAgent.remainingDistance <= _attackRange && distance <= _attackRange)
        {
            _navMeshAgent.isStopped = true;
            CurrentState = state.attack;
        }
        else
        {
            _navMeshAgent.isStopped = false;
            CurrentState = state.chasing;
        }
    }


    void attackPlayer()
    {
        // attacking player

        _target.SendMessage("applyDamage", _attackPower, SendMessageOptions.DontRequireReceiver);

        Debug.Log("monster attacks player");
    }



    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"detect collision {other.gameObject}");


        if (other.tag.Equals("PlayerAttack"))
        {

            if(other.gameObject.TryGetComponent<Bullet>(out Bullet bullet))
            {
                int damage = bullet.damage;
            }

            // apply player attack damage 
            _monsterHP = 0;
        }
    }

}
