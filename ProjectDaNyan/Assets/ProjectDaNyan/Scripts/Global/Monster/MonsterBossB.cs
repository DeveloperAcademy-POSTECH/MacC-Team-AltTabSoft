using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class MonsterBossB : Monster
{
    // Boss Monster state
    private enum BossState
    {
        idle,
        chasing,
        normalAttack,
        readyMachineGun,
        startMachineGun,
        readyFatMan,
        startFatMan,
        dead
    }

    [Header("Monster Status")]
    public MonsterBossBData _bossData = null;

    [Header("Monster Attack")]
    // where bullets are fired 
    [SerializeField] private Transform[] bulletPoints;
    [SerializeField] private Transform[] bulletParents;
    // bullet prefab  
    [SerializeField] private GameObject _monsterBulletPrefab;
    // nuclear bomb prefab 
    [SerializeField] private GameObject _fatManPrefab;
    // gun barrel 
    [SerializeField] private Transform _gunPosition;
    
    
    [Header("Monster Current State")]
    // boss monster current state 
    [SerializeField]
    private BossState _currentState;

    // boss monstser status
    // public float monsterHP;
    // //Bomb explosion objects
    // [Header("Bomb Explosion")]
    // [SerializeField] private GameObject _boom;
    // [SerializeField] private GameObject _boomCollider;
    // [SerializeField] private GameObject _bomb; //Bomb on the Boss Monster
    // [SerializeField] private PlayerAttack _playerAttack;
    // private int _bombLevel;

    // privates variables 
    private float _idleTimeCount = 0;
    private float _skillReadyTimeCount = 0;
    private float _attackTimeCount = 0;
    private float _attackedCount = 0;
    private MonsterAttack _monsterAttack;

    // nav mesh agent settings 
    private NavMeshAgent _navMeshAgent = null;
    private GameObject _target = null;
    // private void Update()
    // {
    //     _bombLevel = _playerAttack.bombLevel;
    // }


    private CharacterController _targetCC;
    

    private void OnEnable()
    {
        // set monster type 
        myType = MonsterType.Boss;
        
        //get Components
        #region Get components

        // find target
        _target = GameObject.FindGameObjectWithTag("Player");

        _targetCC = _target.GetComponent<CharacterController>();
        
        // boss spawn point 
        this.transform.position = _target.transform.position;

        // get NavMeshAgent component 
        _navMeshAgent = GetComponent<NavMeshAgent>();

        // //PlayerAttack Bomb Skill Information
        // _playerAttack = GameObject.Find("PlayerAttackPosition").GetComponent<PlayerAttack>();
        //

        #endregion

        // set boss status

        #region

        monsterHP = _bossData.HP;
        
        #endregion

        // set boss state 
        _currentState = BossState.idle;

        // Make Camera Move To Boss //
        StartCoroutine(CameraFocusOnBoss());
    }


    // Camera moving

    #region Camera Focus on Boss

    IEnumerator CameraFocusOnBoss()
    {
        CameraManager cameraManager = GameObject.FindGameObjectWithTag("MainCamera").gameObject.transform.parent
            .GetComponent<CameraManager>();
        cameraManager.MonsterVCam.Follow = transform;
        cameraManager.MonsterVCam.Priority = 30;

        yield return new WaitForSeconds(2);
        cameraManager.MonsterVCam.Priority = 0;
    }

    #endregion


    // chasing => normal attack * 3 => dash attack => wide attack => chasing 
    private void FixedUpdate()
    {
        switch (_currentState)
        {
            case BossState.idle:

                // time delay 
                _idleTimeCount += Time.deltaTime;

                if (_idleTimeCount >= _bossData.IdleTime)
                {
                    _currentState = BossState.chasing;
                }

                break;

            case BossState.chasing:

                checkAttackDistance();

                // set nav mesh agent settings 
                _navMeshAgent.speed = _bossData.Speed;
                _navMeshAgent.stoppingDistance = _bossData.AttackRange;
                
                _navMeshAgent.SetDestination(_target.transform.position);

                break;

            case BossState.normalAttack:
                // count attack time 
                _attackTimeCount += Time.deltaTime;

                // look at player 
                lookAtPlayer();
                
                // attack interval 
                if (_attackTimeCount >= _bossData.AttackInterval)
                {
                    _attackTimeCount = 0;
                    _attackedCount += 1;
                    attackPlayer(_bossData.AttackSpeed, _target.transform.position);
                }

                if (_attackedCount > _bossData.AttackCount)
                {
                    _attackedCount = 0;
                    _attackTimeCount = 0;
                    _currentState = BossState.readyMachineGun;

                    break;
                }

                checkAttackDistance();

                break;

            // look at target 
            case BossState.readyMachineGun:

                lookAtPlayer();

                _skillReadyTimeCount += Time.deltaTime;

                if (_skillReadyTimeCount > _bossData.ReadyMachineGunTime)
                {
                    _skillReadyTimeCount = 0;
                    _currentState = BossState.startMachineGun;
                    break;
                }
                
                break;

            // dash to target 
            case BossState.startMachineGun:
            
                _attackTimeCount += Time.deltaTime;

                Vector3 rotateMahcinGun = new Vector3(0, 0, 1);
                float rotateSpeed = 40f;
                
                foreach (Transform tr in bulletParents)
                {
                    tr.Rotate(rotateMahcinGun * rotateSpeed);
                }
                // look at player 
                lookAtPlayer();

                
                
                // get player speed and normalize 
                float targetMoveDirLength = _targetCC.velocity.magnitude;
                // set new shooting point
                Vector3 shootingPoint = _target.transform.forward * targetMoveDirLength;
                
                // attack interval 
                if (_attackTimeCount >= _bossData.MachineGunRate)
                {
                    _attackTimeCount = 0;
                    _attackedCount += 1;
                    attackPlayer(_bossData.MachineGunSpeed, shootingPoint);
                }

                if (_attackedCount > _bossData.MachineGunCount)
                {
                    _attackedCount = 0;
                    _attackTimeCount = 0;
                    _currentState = BossState.readyFatMan;

                    break;
                }
                
                
                break;

            case BossState.readyFatMan:
                lookAtPlayer();
                
                _skillReadyTimeCount += Time.deltaTime;

                if (_skillReadyTimeCount > _bossData.ReadyFatManTime)
                {
                    _skillReadyTimeCount = 0;
                    _currentState = BossState.startFatMan;
                    break;
                }
                
                break;

            case BossState.startFatMan:

                _attackTimeCount += Time.deltaTime;
                
                // attack interval 
                if (_attackTimeCount >= _bossData.FatManRate)
                {
                    _attackTimeCount = 0;
                    _attackedCount += 1;
                    makeFatMan();
                }

                if (_attackedCount > _bossData.FatManCount)
                {
                    _attackedCount = 0;
                    _attackTimeCount = 0;
                    _currentState = BossState.chasing;
                    break;
                }
                
                break;

            case BossState.dead:
                break;
        }
    }


    private void checkAttackDistance()
    {
        float distance = Vector3.Distance(this.transform.position, _target.transform.position);

        if (_navMeshAgent.remainingDistance <= _bossData.AttackRange && distance <= _bossData.AttackRange)
        {
            // player is within attack range 
            _navMeshAgent.velocity = Vector3.zero;
            _navMeshAgent.isStopped = true;
            _currentState = BossState.normalAttack;
        }
        else
        {
            // not within attack range
            _navMeshAgent.isStopped = false;
            _currentState = BossState.chasing;
        }
    }

    private void lookAtPlayer()
    {
        Vector3 targetPos = new Vector3(_target.transform.position.x, this.transform.position.y,
            _target.transform.position.z);
        this.transform.LookAt(targetPos);
        _gunPosition.transform.LookAt(targetPos);
    }
    
    private void attackPlayer(float bulletSpeed, Vector3 shootingPos)
    {
        foreach (Transform tr in bulletPoints)
        {
            GameObject bullet = ObjectPoolManager.Inst.BringObject(_monsterBulletPrefab);
            bullet.transform.position = tr.position;
            bullet.GetComponent<MonsterAttack>().Damage = _bossData.AttackPower;
            bullet.transform.LookAt(shootingPos);
            Rigidbody bulletRB = bullet.GetComponent<Rigidbody>();
            bulletRB.velocity = tr.forward * bulletSpeed;
        }
    }


    private void makeFatMan()
    {
        GameObject fatMan = ObjectPoolManager.Inst.BringObject(_fatManPrefab);
        Vector3 bombPosition = new Vector3(_target.transform.position.x, 20f, _target.transform.position.z);
        fatMan.GetComponent<MonsterAttack>().Damage = _bossData.FatManDamage;
        fatMan.transform.position = bombPosition;

        Rigidbody fatManRigidbody = fatMan.GetComponent<Rigidbody>();
        fatManRigidbody.velocity = fatMan.transform.up * -20f;
    }


    // boss is dead
    public void bossStateChangeToDead()
    {
        GameManager.Inst.BossDead();
    }
    
    // collision check 

    // #region Collision check Code
    //
    // private void OnTriggerEnter(Collider other)
    // {
    //     if (other.tag.Equals("PlayerAttack"))
    //     {
    //         // get bullet damage 
    //         if (other.gameObject.TryGetComponent(out Bullet bullet))
    //         {
    //             // apply player attack damage 
    //             applyDamage(bullet.damage);
    //
    //             if (this.gameObject.transform.Find("BombOnMonster") != null &&
    //                 this.gameObject.transform.Find("BombOnMonster").gameObject.activeSelf == true)
    //             {
    //                 _bomb = this.gameObject.transform.Find("BombOnMonster").gameObject;
    //                 bullet.bombStack += 1;
    //                 StartCoroutine(bombExplosion(bullet, _bomb, _bombLevel, 0.5f));
    //             }
    //
    //             if (bullet.type == Bullet.Type.Bomb && this.gameObject.transform.Find("BombOnMonster") != null &&
    //                 this.gameObject.transform.Find("BombOnMonster").gameObject.activeSelf == false)
    //             {
    //                 _bomb.SetActive(true);
    //             }
    //         }
    //         // if bullet doesn't have damage 
    //         else
    //         {
    //             applyDamage(1);
    //         }
    //     }
    // }
    //
    // private void OnTriggerStay(Collider other)
    // {
    //     if (other.tag.Equals("PlayerAttack"))
    //     {
    //         // get bullet damage 
    //         if (other.gameObject.TryGetComponent(out Bullet bullet))
    //         {
    //             // apply player attack damage 
    //             applyDamage(bullet.damage);
    //         }
    //         // if bullet doesn't have damage 
    //         else
    //         {
    //             applyDamage(1);
    //         }
    //     }
    // }
    //
    // #endregion
    //
    // IEnumerator bombExplosion(Bullet bullet, GameObject bomb, int bombLevel, float boomSize)
    // {
    //     if (bullet.bombStack > 2) //default 20
    //     {
    //         if (bombLevel > 4)
    //             bombLevel = 4;
    //         bullet.bombStack = 0;
    //         //몬스터 위에 있는 폭탄 비활성
    //         ObjectPoolManager.Inst.DestroyObject(bomb);
    //         //폭발 파티클 이펙트
    //         GameObject boomEffect = ObjectPoolManager.Inst.BringObject(_boom);
    //         boomEffect.transform.localScale = new Vector3(boomSize + (0.25f * bombLevel) , boomSize + (0.25f * bombLevel), boomSize + (0.25f * bombLevel));
    //         boomEffect.transform.position = this.gameObject.transform.position + new Vector3(0, 1f, 0);
    //
    //         //터지는 순간 위에서 안보이는 Collider가 떨어지면서 Trigger 발동
    //         GameObject boomCollider = ObjectPoolManager.Inst.BringObject(_boomCollider);
    //         boomCollider.transform.localScale = new Vector3(boomSize + (0.25f * bombLevel), boomSize + (0.25f * bombLevel), boomSize + (0.25f * bombLevel));
    //         boomCollider.transform.position = this.gameObject.transform.position + new Vector3(0, 10, 0);
    //         Rigidbody boomColliderRigid = boomCollider.GetComponent<Rigidbody>();
    //         boomColliderRigid.velocity = boomCollider.transform.up * -100f;
    //
    //         yield return new WaitForSeconds(1f);
    //         ObjectPoolManager.Inst.DestroyObject(boomEffect);
    //         ObjectPoolManager.Inst.DestroyObject(boomCollider);
    //     }
    // }
    //
    // apply damage 
    protected override void applyDamage(float damage)
    {
        if (_currentState == BossState.dead)
        {
            return;
        }
    
        monsterHP -= damage;
    
        if (monsterHP <= 0)
        {
            _currentState = BossState.dead;
            GameManager.Inst.BossDead();
            return;
        }
    }
}