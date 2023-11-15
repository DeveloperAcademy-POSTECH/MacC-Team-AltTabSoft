using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;

public class MonsterNormal : Monster
{
    public IObjectPool<GameObject> myPool { get; set; }

    [SerializeField] private MonsterData _monsterData;

    // nav mesh related variables 
    private NavMeshAgent _navMeshAgent = null;
    private Transform _target = null;

    [Header("Prefabs")]
    [SerializeField] private GameObject _monsterBulletPrefab;
    [SerializeField] private GameObject _expBox;
    
    [Header("Monster Data")]
    [SerializeField] public float MonsterHP;
    
    [SerializeField] private state _currentState;
    [SerializeField] private Renderer _renderer;
    [SerializeField] private Transform _attackPoint;
    
    
    //부착된 폭탄이 터질 때 폭발효과
    [Header("Bomb Effects")]
    [SerializeField] private GameObject _boom;
    [SerializeField] private GameObject _boomCollider;
    
    // private variables 
    private float _attacktime;
    private MonsterAttack _monsterAttack;
    
    // Monster state
    public enum state
    {
        chasing,
        attack,
        dead
    }


    private void Awake()
    {
        // set monster type
        myType = _monsterData.monsterType;
        
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
        if(!_navMeshAgent.isOnNavMesh)
        {
            return;
        }
        
        // current state as chasing 
        _currentState = state.chasing;

        // set monster HP 
        MonsterHP = _monsterData.hp;
        
        // set material color 
        _renderer.material.color = Color.white;

        // set nav mesh agent values 
        _navMeshAgent.stoppingDistance = _monsterData.attackRange;
        _navMeshAgent.speed = _monsterData.speed;
        _navMeshAgent.SetDestination(_target.position);

        // set delegate 
        GameManager.Inst.delegateGameState += PrepareBossStage;

        // start coroutine 
        StartCoroutine(monsterState());
    }

    IEnumerator monsterState()
    {

        while (MonsterHP > 0)
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


        if(_attacktime >= _monsterData.attackInterval)
        {
            _attacktime = 0;
            attackPlayer();
        }
        yield return null;
    }

    private void attackPlayer()
    {
        // attacking player
        GameObject bullet = ObjectPoolManager.Inst.BringObject(_monsterBulletPrefab);
        bullet.transform.position = _attackPoint.position;
        bullet.GetComponent<MonsterAttack>().Damage = _monsterData.attackPower;
        bullet.transform.LookAt(_target);

        Vector3 bulletDir = _target.position - this.transform.position;

        Rigidbody bulletRB = bullet.GetComponent<Rigidbody>();
        bulletRB.velocity = bulletDir * _monsterData.attackSpeed;
    }

    IEnumerator dead()
    {
        GameManager.Inst.delegateGameState -= PrepareBossStage;

        GameObject expBox = ObjectPoolManager.Inst.BringObject(_expBox);
        EXPBox expBoxData = expBox.GetComponent<EXPBox>();

        // drop exp 
        expBoxData.exp = _monsterData.exp;
        expBoxData.parentsVelocity = _navMeshAgent.velocity;
        expBox.transform.position = this.transform.position + Vector3.up * 2f;

        //몬스터가 죽을 시 폭탄 터짐
        if (this.gameObject.transform.Find("BombOnMonster") != null)
        {
            //폭발 파티클 이펙트
            GameObject boomEffect = ObjectPoolManager.Inst.BringObject(_boom);
            boomEffect.transform.position = this.gameObject.transform.position;

            //터지는 순간 위에서 안보이는 Collider가 떨어지면서 Trigger 발동
            GameObject boomCollider = ObjectPoolManager.Inst.BringObject(_boomCollider);
            boomCollider.transform.position = this.gameObject.transform.position + new Vector3(0,10,0);
            Rigidbody boomColliderRigid = boomCollider.GetComponent<Rigidbody>();
            boomColliderRigid.velocity = boomCollider.transform.up * -100f;


            yield return new WaitForSeconds(0.2f);
            ObjectPoolManager.Inst.DestroyObject(this.gameObject);
            ObjectPoolManager.Inst.DestroyObject(boomEffect);
            ObjectPoolManager.Inst.DestroyObject(boomCollider);

        }
        else
        {
            ObjectPoolManager.Inst.DestroyObject(this.gameObject);
        }

        yield return null;
    }

    private void checkAttackDistance()
    {

        float distance = Vector3.Distance(this.transform.position, _target.transform.position);

        if (distance >= 45) // 수정 필요
        {
            transform.position = _target.position + _target.forward * 30f; 
        }

        else if (_navMeshAgent.remainingDistance <= _monsterData.attackRange && distance <= _monsterData.attackRange)
        {
            _navMeshAgent.isStopped = true;
            _currentState = state.attack;
        }
        else
        {
            _navMeshAgent.isStopped = false;
            _currentState = state.chasing;
        }
    }


    public void PrepareBossStage(GameState gameState)
    {
        if(gameState == GameState.bossReady)
        {
            ObjectPoolManager.Inst.DestroyObject(this.gameObject);
        }
    }


    IEnumerator monsterHit()
    {
        _renderer.material.color = Color.red;
        yield return new WaitForSeconds(0.25f);
        _renderer.material.color = Color.white;
    }
    
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("PlayerAttack"))
        {
            // get bullet damage 
            if (other.gameObject.TryGetComponent(out Bullet bullet))
            {
                // apply player attack damage 
                MonsterHP -= bullet.damage;
            }
            else
            {
                MonsterHP -= 1;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag.Equals("PlayerAttack"))
        {
            StartCoroutine(monsterHit());
            
            // get bullet damage 
            if (other.gameObject.TryGetComponent(out Bullet bullet))
            {
                // apply player attack damage 
                MonsterHP -= bullet.damage;
            }
            else
            {
                MonsterHP -= 1;
            }
        }
    }
}