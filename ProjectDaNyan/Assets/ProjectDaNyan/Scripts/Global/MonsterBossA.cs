using System.Collections;
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
    [SerializeField] private float _attackPower;
    [SerializeField] private float _monsterHP;
    [SerializeField] private float _monsterSpeed;
    [SerializeField] private float _attackRange;
    [SerializeField] private float _attackInterval;
    //[SerializeField] private float _attackSpeed;
    [SerializeField] private float _normalAttackCount;
    [SerializeField] private float _readyDashTime;
    [SerializeField] private float _dashPoint;
    [SerializeField] private float _dashSpeed;
    [SerializeField] private float _dashCount;
    [SerializeField] private float _idleTime;

    [Header("Monster Attack")]
    [SerializeField] private GameObject skillWaveBlast;
    [SerializeField] private CapsuleCollider[] _pawsColliers;
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

    // where bullets are fired 
    public Transform _attackPoint = null;

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
        _monsterHP = _bossData.HP;
        _monsterSpeed = _bossData.Speed;
        _attackRange = _bossData.AttackRange;
        _attackInterval = _bossData.AttackInterval;
        _normalAttackCount = _bossData.AttackCount;
        _readyDashTime = _bossData.ReadyDashTime;
        _dashPoint = _bossData.DashPoint;
        _dashSpeed = _bossData.DashSpeed;
        _dashCount = _bossData.DashCount;
        _idleTime = _bossData.IdleTime;
        #endregion

        // set monster type 
        myType = MonsterType.Boss;
        
        
        // set capsule collider = off 
        pawsColliderControl(false);
        
        
        // get line renderer 
        _line.startWidth = _line.endWidth = 0.2f;
        _line.material.color = Color.blue;
        _line.enabled = false;

        // set boss state 
        _currentState = BossState.idle;

        // set navmeshagent settings 
        _navMeshAgent.speed = _monsterSpeed;
        _attackRange = _bossData.AttackRange;
        _navMeshAgent.stoppingDistance = _attackRange;

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

        _animator.SetTrigger("Roar");
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

                // Roar animation 
                _animator.SetTrigger("Roar");

                // time delay 
                _idleTimeCount += Time.deltaTime;

                if (_idleTimeCount >= _idleTime)
                {
                    _currentState = BossState.chasing;
                }

                break;

            case BossState.chasing:

                pawsColliderControl(false);
                
                _navMeshAgent.isStopped = false;
                _navMeshAgent.speed = _monsterSpeed;
                _navMeshAgent.stoppingDistance = _attackRange;

                checkAttackDistance();

                _navMeshAgent.SetDestination(_targetTransform.position);

                // draw path 
                // StartCoroutine(makePathCoroutine());

                break;

            case BossState.normalAttack:
                // normal attack animation

                pawsColliderControl(true);

                // count attack time 
                _attackTimeCount += Time.deltaTime;

                // look at player 
                lookAtPlayer();

                // attack interval 
                if (_attackTimeCount >= _attackInterval)
                {
                    _attackTimeCount = 0;
                    _attackedCount += 1;
                    attackPlayer();
                }

                // normal attack to dash attack 
                if (_attackedCount > _normalAttackCount)
                {
                    _attackedCount = 0;
                    _attackTimeCount = 0;
                    _currentState = BossState.readyDashAttack;

                    // Roar animation 
                    _animator.SetTrigger("Roar");

                    break;
                }

                checkAttackDistance();

                break;

            // look at target 
            case BossState.readyDashAttack:

                // don't play dash animation 
                _animator.SetBool("Dash", false);
               
                lookAtPlayer();

                _dashReadyTimeCount += Time.deltaTime;

                if (_dashReadyTimeCount > _readyDashTime)
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
                if (_dashed > _dashCount)
                {
                    _dashed = 0;

                    _navMeshAgent.stoppingDistance = _attackRange;
                    _navMeshAgent.speed = _monsterSpeed;

                    // attack big wave 
                    _currentState = BossState.readyBigWave;
                    break;
                }

                // set dash values  
                _navMeshAgent.speed = _dashSpeed;
                _navMeshAgent.isStopped = false;
                _navMeshAgent.stoppingDistance = _dashStoppingDistance;

                // get player speed and normalize 
                float targetMoveDirLength = _targetCC.velocity.magnitude;

                // set dash direction
                float dashPoint = targetMoveDirLength * _dashPoint;

                // dash in front of player 
                _dashDirection = _targetTransform.position + _targetTransform.forward * dashPoint;

                // set current state 
                _currentState = BossState.onDashAttack;

                // dash animation
                _animator.SetBool("Dash", true);
                _animator.speed = 2;

                break;

            case BossState.onDashAttack:
                agentRotation();
                dashAttack(_dashDirection);
                // StartCoroutine(makePathCoroutine());

                break;

            case BossState.waveBlast:
                lookAtPlayer();
                break;

            case BossState.readyBigWave:
                lookAtPlayer();
                _animator.SetTrigger("Roar");

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
                    _bigWaved++;

                    // reset time
                    _bigWaveTime = 0;

                    // change state 
                    _currentState = BossState.onBigWaveAttack;

                    // play animation 
                    _animator.SetTrigger("Jump");
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

        if (_navMeshAgent.remainingDistance <= _attackRange && distance <= _attackRange)
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
            _animator.speed = 2f;
            _currentState = BossState.chasing;
        }
    }

    private void lookAtPlayer()
    {
        Vector3 targetPos = new Vector3(_targetTransform.position.x, this.transform.position.y, _targetTransform.position.z);
        this.transform.LookAt(targetPos);
    }

    // normal attack
    private void attackPlayer()
    {
        if (_attackedCount % 2 == 0)
        {
            _animator.SetTrigger("AttackRH");
        }
        else
        {
            _animator.SetTrigger("AttackLH");
        }
    }


    // attack dash 
    private void dashAttack(Vector3 dashPos)
    {
        // dash to current target position 
        _navMeshAgent.SetDestination(dashPos);

        if (!_navMeshAgent.pathPending)
        {
            if(_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
            {
                // stop dash and change boss state to waveBlast 
                stopDashAndBlastAttack();
            }
        }
    }

    private void stopDashAndBlastAttack()
    {
        _navMeshAgent.velocity = Vector3.zero;
        _navMeshAgent.isStopped = true;
        
        // turn off the dash animation 
        _animator.SetBool("Dash", false);

        waveBlastStart();

        _currentState = BossState.waveBlast;
    }

    public void StartWaveBlastAttack()
    {
        // create wave blast 
        _skillMonsterBlast.StartWaveBlastAttack(SkillType.WaveBlast);
    }

    private void waveBlastStart()
    {
        // save state to change after wave blast end 
        _lastState = BossState.readyDashAttack;

        // play animation
        _animator.SetTrigger("AttackBothPaws");
    }

    // when wave blast end => EvenHandler => change state
    private void waveBlastEnd()
    {
        _currentState = _lastState;
    }

    public void StartBigWaveAttack()
    {
        _skillMonsterBlast.StartWaveBlastAttack(SkillType.BigWave);
    }

    // boss is dead
    public void bossStateChangeToDead()
    {
        GameManager.Inst.BossDead();
    }



    //draw path
     #region Draw navmeshpath with Line Renderer  
     IEnumerator makePathCoroutine()
    {
        _line.SetPosition(0, this.transform.position);
        _line.enabled = true;

        while (Vector3.Distance(this.transform.position, _navMeshAgent.destination) > 0.1f)
        {
             _line.SetPosition(0, this.transform.position);

            drawPath();

            yield return null;
        }

        _line.enabled = false;
    }

    private void drawPath()
    {
        int length = _navMeshAgent.path.corners.Length;
        _line.positionCount = length;

        for (int i = 0; i < length; ++i)
        {
            _line.SetPosition(i, _navMeshAgent.path.corners[i]);
        }
    }
    #endregion



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
                
                if (this.gameObject.transform.Find("BombOnMonster") != null && this.gameObject.transform.Find("BombOnMonster").gameObject.activeSelf == true)
                {
                    _bomb = this.gameObject.transform.Find("BombOnMonster").gameObject;
                    bullet.bombStack += 1;
                    Debug.Log($"bomb Stack is : {bullet.bombStack}");
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
    #endregion

    IEnumerator bombExplosion(Bullet bullet, GameObject bomb)
    {
        if (bullet.bombStack > 20)
        {
            bullet.bombStack = 0;
            //몬스터 위에 있는 폭탄 비활성
            ObjectPoolManager.Inst.DestroyObject(bomb);
            //폭발 파티클 이펙트
            GameObject boomEffect = ObjectPoolManager.Inst.BringObject(_boom);
            boomEffect.transform.position = this.gameObject.transform.position + new Vector3(0,1f,0);

            //터지는 순간 위에서 안보이는 Collider가 떨어지면서 Trigger 발동
            GameObject boomCollider = ObjectPoolManager.Inst.BringObject(_boomCollider);
            boomCollider.transform.position = this.gameObject.transform.position + new Vector3(0, 10, 0);
            Rigidbody boomColliderRigid = boomCollider.GetComponent<Rigidbody>();
            boomColliderRigid.velocity = boomCollider.transform.up * -100f;

            yield return new WaitForSeconds(0.2f);
            ObjectPoolManager.Inst.DestroyObject(boomEffect);
            ObjectPoolManager.Inst.DestroyObject(boomCollider);
        }
    }

    // apply damage 
    private void applyDamage(float damage)
    {
        if(_currentState == BossState.dead)
        {
            return;
        }

        _monsterHP -= damage;

        if(_monsterHP <= 0)
        {
            // play dead animation 
            _animator.SetBool("Death", true);

            _currentState = BossState.dead;
            return;
        }
    }
}
