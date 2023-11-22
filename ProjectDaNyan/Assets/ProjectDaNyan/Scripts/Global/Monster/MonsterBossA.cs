using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;

public class MonsterBossA : Monster
{
    // Boss Monster state
    private enum BossState
    {
        idle,
        chasing,
        normalAttack,
        readyDashAttack,
        startDashAttack,
        onDashAttack,
        waveBlast,
        readyBigWave,
        bigWave,
        onBigWaveAttack,
        dead
    }

    [Header("Monster Status")]
    public MonsterBossAData _bossData = null;
    
    // boss monstser status
    public float monsterHP;

    [Header("Monster Attack")]
    [SerializeField] private GameObject skillWaveBlast;
    [SerializeField] private CapsuleCollider[] _pawsColliers;
    [SerializeField] private TrailRenderer[] _pawsTrailRenderers;
    private MonsterSkillWaveBlast _skillMonsterBlast;

    [Header("Monster Current State")]
    // boss monster current state 
    [SerializeField] private BossState _currentState;
    
    [Header("Monster Animation")]
    [SerializeField] Animator _animator;

    //Bomb explosion objects
    [Header("Bomb Explosion")]
    [SerializeField] GameObject _boom;
    [SerializeField] GameObject _boomCollider;
    [SerializeField] GameObject _bomb;//Bomb on the Boss Monster

    // privates variables 
    private float _idleTimeCount = 0;
    private float _dashReadyTimeCount = 0;
    private float _attackTimeCount = 0;
    private float _attackedCount = 0;
    private float _dashed = 0;
    private Vector3 _dashDirection = Vector3.zero;
    private float _dashStoppingDistance = 1f;
    private float _bigWaveTime = 0f;
    private float _bigWaved = 0f;
    private BossState _lastState;
    private MonsterAttack _monsterAttack;

    // nav mesh agent settings 
    private NavMeshAgent _navMeshAgent = null;
    private GameObject _target = null;
    private Transform _targetTransform;
    private CharacterController _targetCC;
    private LineRenderer _line;

   
    private void OnEnable()
    {

        //get Components
        #region Get components

        // find target
        _target = GameObject.FindGameObjectWithTag("Player");

        // get target transform 
        _targetTransform = _target.GetComponent<PlayerController>().transform;

        // get target rigidbody
        _targetCC = _target.GetComponent<CharacterController>();

        // boss spawn point 
        this.transform.position = _targetTransform.position;

        // get NavMeshAgent component 
        _navMeshAgent = GetComponent<NavMeshAgent>();

        // get animator
        _animator = GetComponentInChildren<Animator>();
      
        // get line renderer 
        _line = GetComponent<LineRenderer>();

        // skill wave blast
        _skillMonsterBlast = skillWaveBlast.GetComponent<MonsterSkillWaveBlast>();

        // get monster damage
        _monsterAttack = GetComponent<MonsterAttack>();
        
        #endregion

        // set boss status
        #region
        _monsterAttack.Damage = _bossData.AttackPower;
        monsterHP = _bossData.HP;
        #endregion

        // set monster type 
        myType = MonsterType.Boss;
        
        
        // set capsule collider = off 
        pawsColliderControl(false);

        // set boss state 
        _currentState = BossState.idle;

        // set navmeshagent settings 
        _navMeshAgent.speed = _bossData.Speed;
        _navMeshAgent.stoppingDistance = _bossData.AttackRange;

        // unity event add listener 
        _skillMonsterBlast.EventHandlerWaveBlastEnd.AddListener(waveBlastEnd);

        // Make Camera Move To Boss //
        StartCoroutine(CameraFocusOnBoss());
        
    }

    IEnumerator bossAppear()
    {
        _navMeshAgent.enabled = false;
        this.transform.LookAt(_targetTransform);
        _navMeshAgent.enabled = true;
        yield return null;
    }


    // Camera moving
    #region Camera Focus on Boss 
    IEnumerator CameraFocusOnBoss()
    {
        CameraManager cameraManager = GameObject.FindGameObjectWithTag("MainCamera").gameObject.transform.parent.GetComponent<CameraManager>();
        cameraManager.MonsterVCam.Follow = transform;
        cameraManager.MonsterVCam.Priority = 30;

        StartCoroutine(bossAppear());

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

                pawsColliderControl(false);
                
                _navMeshAgent.isStopped = false;
                _navMeshAgent.speed = _bossData.Speed;
                _navMeshAgent.stoppingDistance = _bossData.AttackRange;

                checkAttackDistance();

                _navMeshAgent.SetDestination(_targetTransform.position);

                // draw path 
                // StartCoroutine(makePathCoroutine());

                break;

            case BossState.normalAttack:
                BearClawTrailRenderer(BearPaw.Both);
                
                _navMeshAgent.velocity = Vector3.zero;
                
                // normal attack animation
                pawsColliderControl(true);

                // count attack time 
                _attackTimeCount += Time.deltaTime;

                // look at player 
                lookAtPlayer();

                // attack interval 
                if (_attackTimeCount >= _bossData.AttackInterval)
                {
                    _attackTimeCount = 0;
                    _attackedCount += 1;
                    attackPlayer();
                }

                // normal attack to dash attack 
                if (_attackedCount > _bossData.AttackCount)
                {
                    _attackedCount = 0;
                    _attackTimeCount = 0;
                    _currentState = BossState.readyDashAttack;

                    break;
                }

                checkAttackDistance();

                break;

            // look at target 
            case BossState.readyDashAttack:
                
                // normal attack animation
                pawsColliderControl(false);
                
                _navMeshAgent.velocity = Vector3.zero;
               
                lookAtPlayer();

                _dashReadyTimeCount += Time.deltaTime;

                if (_dashReadyTimeCount > _bossData.ReadyDashTime)
                {
                    _dashReadyTimeCount = 0;
                    _currentState = BossState.startDashAttack;
                    break;
                }

                break;

            // dash to target 
            case BossState.startDashAttack:
                // add dash count 
                _dashed++;

                // if boss dashed more than dash count -> change state 
                if (_dashed > _bossData.DashCount)
                {
                    _dashed = 0;

                    _navMeshAgent.stoppingDistance = _bossData.AttackRange;
                    _navMeshAgent.speed = _bossData.Speed;

                    // attack big wave 
                    _currentState = BossState.readyBigWave;
                    break;
                }

                // set dash values  
                _navMeshAgent.isStopped = false;
                _navMeshAgent.speed = _bossData.DashSpeed;
                _navMeshAgent.stoppingDistance = _dashStoppingDistance;

                // // get player speed and normalize 
                // float targetMoveDirLength = _targetCC.velocity.magnitude;
                //
                // // set dash direction
                // float dashPoint = targetMoveDirLength * _bossData.DashPoint;
                //
                // // dash in front of player 
                // _dashDirection = _targetTransform.position + _targetTransform.forward * dashPoint;

                // set current state 
                _currentState = BossState.onDashAttack;

                _animator.SetBool("Dash", true);
                
                
                break;

            case BossState.onDashAttack:
                // play animation
                _animator.SetTrigger("AttackBothPaws");
                
                dashAttack(_targetTransform.position);
                break;

            case BossState.waveBlast:
                lookAtPlayer();
                break;

            case BossState.readyBigWave:
                              
                // turn off trail renderer 
                BearClawTrailRenderer(BearPaw.off);
                
                _navMeshAgent.velocity = Vector3.zero;
                
                lookAtPlayer();
                _currentState = BossState.bigWave;

                // save state 
                _lastState = BossState.bigWave;
                break;

            case BossState.bigWave:

                // change state 
                if (_bigWaved >= _bossData.BigWaveCount)
                {
                    _bigWaved = 0;
                    _currentState = BossState.chasing;
                    break;
                }
           
                _bigWaveTime += Time.deltaTime;

                if(_bigWaveTime >= _bossData.BigWaveInterval)
                {
                    // play animation 
                    _animator.SetTrigger("Jump");
                    
                    _bigWaved++;

                    // reset time
                    _bigWaveTime = 0;

                    // change state 
                    _currentState = BossState.onBigWaveAttack;
                }

                break;

            case BossState.onBigWaveAttack:
                lookAtPlayer();
                break;

            case BossState.dead:
                break;
        }
    }



    // check distance between boss and player 

    private void agentRotation()
    {
        // agent moving direction
        Vector3 direction = _navMeshAgent.desiredVelocity;
        // calculate quaternion 
        Quaternion targetAngle = Quaternion.LookRotation(direction);
        // smooth rotation 
        transform.rotation = Quaternion.Slerp(transform.rotation, targetAngle, Time.deltaTime);
    }



    private void checkAttackDistance()
    {
        float distance = Vector3.Distance(this.transform.position, _target.transform.position);

        if (distance <= _bossData.AttackRange || _navMeshAgent.remainingDistance <= _bossData.AttackRange)
        {
            // player is within attack range 
            _animator.SetBool("Chase", false);
            _navMeshAgent.velocity = Vector3.zero;
            _currentState = BossState.normalAttack;
        }
        else
        {
            // not within attack range
            // walking animation
            _animator.SetBool("Chase", true);
            _currentState = BossState.chasing;
        }
    }

    private void lookAtPlayer()
    {
        Vector3 targetPos = new Vector3(_targetTransform.position.x, this.transform.position.y, 
            _targetTransform.position.z);
        this.transform.LookAt(targetPos);
    }

    // normal attack
    private void attackPlayer()
    {
        // normal attack animation
        pawsColliderControl(true);
        
        if (_attackedCount % 2 == 0)
        {
            _animator.SetTrigger("AttackLH");
        }
        else
        {
            _animator.SetTrigger("AttackRH");
        }
    }

    public enum BearPaw{
        LH,
        RH,
        Both,
        off
    }
    
    public void BearClawTrailRenderer(BearPaw paw)
    {
        _pawsTrailRenderers[0].emitting = paw == BearPaw.LH | paw == BearPaw.Both;
        _pawsTrailRenderers[1].emitting = paw == BearPaw.RH | paw == BearPaw.Both;
    }
    
    
    
    // attack dash 
    private void dashAttack(Vector3 dashPos)
    {
        // don't play dash animation 
        _animator.SetBool("Dash", false);
        
        // dash to current target position 
        _navMeshAgent.SetDestination(dashPos);

        float distance = Vector3.Distance(_target.transform.position, this.transform.position);

        if (distance <= _bossData.AttackRange)
        {
            stopDashAndBlastAttack();
        }
        
        // if (!_navMeshAgent.pathPending)
        // {
        //     if(_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
        //     {
        //         // stop dash and change boss state to waveBlast 
        //         stopDashAndBlastAttack();
        //     }
        // }
    }

    private void stopDashAndBlastAttack()
    {
        
        _navMeshAgent.isStopped = true;
        _navMeshAgent.velocity = Vector3.zero;

        waveBlastStart();

        _currentState = BossState.waveBlast;
    }

    public void StartWaveBlastAttack()
    {
        // create wave blast 
        _skillMonsterBlast.StartWaveBlastAttack(WaveType.WaveBlast);
    }

    private void waveBlastStart()
    {
        // save state to change after wave blast end 
        _lastState = BossState.readyDashAttack;
        
        // normal attack animation
        pawsColliderControl(true);
    }

    // when wave blast end => EvenHandler => change state
    private void waveBlastEnd()
    {
        _currentState = _lastState;
    }

    public void StartBigWaveAttack()
    {
        _skillMonsterBlast.StartWaveBlastAttack(WaveType.BigWave);
    }

    // boss is dead
    public void bossStateChangeToDead()
    {
        GameManager.Inst.BossDead();
    }
    

    // capsule collider on/off 
    private void pawsColliderControl(bool isOn)
    {
        foreach (CapsuleCollider cc in _pawsColliers)
        {
            cc.enabled = isOn;
        }
    }
    
    // collision check 
    #region Collision check Code 
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("PlayerAttack"))
        {
            // get bullet damage 
            if (other.gameObject.TryGetComponent(out Bullet bullet))
            {
                // apply player attack damage 
                applyDamage(bullet.damage);

                // bomb explosion 
                if (this.gameObject.transform.Find("BombOnMonster") != null && this.gameObject.transform.Find("BombOnMonster").gameObject.activeSelf == true)
                {
                    _bomb = this.gameObject.transform.Find("BombOnMonster").gameObject;
                    bullet.bombStack += 1;
                    if(bullet.bombStack > 20)
                        StartCoroutine(bombExplosion(bullet,_bomb));
                }

                if (bullet.type == Bullet.Type.Bomb && this.gameObject.transform.Find("BombOnMonster") != null && this.gameObject.transform.Find("BombOnMonster").gameObject.activeSelf == false)
                {
                    _bomb.SetActive(true);
                }
            }
            // if bullet doesn't have damage 
            else
            {
                applyDamage(1);
            }
        }

        if(_currentState == BossState.onDashAttack)
        {
            if (other.tag.Equals("Wall"))
            {
                stopDashAndBlastAttack();
            }
        }
    }

  
    
       
    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.tag.Equals("Player"))
        {
            _skillMonsterBlast.BlastSphereCollider.enabled = false;
            _skillMonsterBlast.isCollided = true;
            
            // normal attack animation
            pawsColliderControl(false);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag.Equals("PlayerAttack"))
        {
            // get bullet damage 
            if (other.gameObject.TryGetComponent(out Bullet bullet))
            {
                // apply player attack damage 
                applyDamage(bullet.damage);
            }
            // if bullet doesn't have damage 
            else
            {
                applyDamage(1);
            }
        }
    }
    #endregion

    // bomb explosion
    IEnumerator bombExplosion(Bullet bullet, GameObject bomb)
    {
        //몬스터 위에 있는 폭탄 비활성
        ObjectPoolManager.Inst.DestroyObject(bomb);
        //폭발 파티클 이펙트
        GameObject boomEffect = ObjectPoolManager.Inst.BringObject(_boom);
        boomEffect.transform.position = this.gameObject.transform.position + new Vector3(0, 1f, 0);

        //터지는 순간 위에서 안보이는 Collider가 떨어지면서 Trigger 발동
        GameObject boomCollider = ObjectPoolManager.Inst.BringObject(_boomCollider);
        boomCollider.transform.position = this.gameObject.transform.position + new Vector3(0, 10, 0);
        Rigidbody boomColliderRigid = boomCollider.GetComponent<Rigidbody>();
        boomColliderRigid.velocity = boomCollider.transform.up * -100f;

        yield return new WaitForSeconds(0.2f);
        ObjectPoolManager.Inst.DestroyObject(boomEffect);
        ObjectPoolManager.Inst.DestroyObject(boomCollider);

        bullet.bombStack = 0;
    }

    // apply damage 
    private void applyDamage(float damage)
    {
        if(_currentState == BossState.dead)
        {
            return;
        }

        monsterHP -= damage;

        if(monsterHP <= 0)
        {
            // play dead animation 
            _animator.SetBool("Death", true);

            _currentState = BossState.dead;
            return;
        }
    }
}
