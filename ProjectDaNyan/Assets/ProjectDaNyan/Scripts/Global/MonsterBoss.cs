using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;
using UnityEngine.Events;
using UnityEngine.UIElements;


public class MonsterBoss : MonoBehaviour
{
    public IObjectPool<GameObject> myPool { get; set; }

    [SerializeField] private MonsterBossData _bossData = null;
 

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
        wideAttack,
        dead
    }

    [Header("Monster Status")]
    // boss monstser status
    [SerializeField] private float _attackPower;
    [SerializeField] private float _monsterHP;
    [SerializeField] private float _monsterSpeed;
    [SerializeField] private float _attackRange;
    [SerializeField] private float _attackInterval;
    [SerializeField] private float _attackSpeed;
    [SerializeField] private float _normalAttackCount;
    [SerializeField] private float _readyDashTime;
    [SerializeField] private float _dashPoint;
    [SerializeField] private float _dashSpeed;
    [SerializeField] private float _dashCount;
    [SerializeField] private float _idleTime;

    [Header("Monster Attack")]
    [SerializeField] private GameObject _monsterBulletPrefab = null;
    [SerializeField] private GameObject skillWaveBlast;
    private MonsterSkillWaveBlast _skillMonsterBlast;

    [Header("Monster Current State")]
    // boss monster current state 
    [SerializeField] private BossState _currentState;


    [Header("Monster Animation")]
    [SerializeField] Animator _animator;

    // where bullets are fired 
    public Transform _attackPoint = null;

    // privates variables 
    private float _idleTimeCount = 0;
    private float _dashReadyTimeCount = 0;
    private float _attackTimeCount = 0;
    private float _attackedCount = 0;
    private float _dashed = 0;
    private Vector3 _dashDirection = Vector3.zero;
    private float _dashStoppingDistance = 10f;



    // nav mesh agent settings 
    private NavMeshAgent _navMeshAgent = null;
    private GameObject _target = null;
    private Transform _targetTransform;
    private CharacterController _targetCC;
    private LineRenderer _line;

    // animation settings
    #region Animator Hash values 
    const string animtorBaseLayer = "Base Layer";
    private int _idleHash = Animator.StringToHash(animtorBaseLayer + ".Idle");
    private int _combatIdleHash = Animator.StringToHash(animtorBaseLayer + ".Combat_Idle");
    private int _chaseHash = Animator.StringToHash(animtorBaseLayer + ".Chase");
    private int dashHash = Animator.StringToHash(animtorBaseLayer + ".Dash");
    private int _attackRHHash = Animator.StringToHash(animtorBaseLayer + ".AttackRH");
    private int _attackLHHash = Animator.StringToHash(animtorBaseLayer + ".AttackLH");
    private int _attackBothPawsHash = Animator.StringToHash(animtorBaseLayer + ".AttackBothPaws");
    private int _roarHash = Animator.StringToHash(animtorBaseLayer + ".Roar");
    private int _jumpHash = Animator.StringToHash(animtorBaseLayer + ".Jump");
    private int _deathHash = Animator.StringToHash(animtorBaseLayer + ".Death");
    #endregion





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

        #endregion

        // set boss status
        #region
        _attackPower = _bossData.AttackPower;
        _monsterHP = _bossData.HP;
        _monsterSpeed = _bossData.Speed;
        _attackRange = _bossData.AttackRange;
        _attackInterval = _bossData.AttackInterval;
        _attackSpeed = _bossData.AttackSpeed;
        _normalAttackCount = _bossData.AttackCount;
        _readyDashTime = _bossData.ReadyDashTime;
        _dashPoint = _bossData.DashPoint;
        _dashSpeed = _bossData.DashSpeed;
        _dashCount = _bossData.DashCount;
        _idleTime = _bossData.IdleTime;
        #endregion

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
        //_navMeshAgent.updateRotation = false;   // enable rotation 

        
        //_animator.SetTrigger("Roar");

        // unity event add listener 
        _skillMonsterBlast.EventHandlerWaveBlastEnd.AddListener(waveBlastEnd);

        // Make Camera Move To Boss //
        StartCoroutine(CameraFocusOnBoss());

        
    }


    // Camera moving
    #region Camera Focus on Boss 
    IEnumerator CameraFocusOnBoss()
    {
        CameraManager cameraManager = GameObject.FindGameObjectWithTag("MainCamera").gameObject.transform.parent.GetComponent<CameraManager>();
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

                _navMeshAgent.isStopped = false;
                _navMeshAgent.speed = _monsterSpeed;
                _navMeshAgent.stoppingDistance = _attackRange;

                checkAttackDistance();

                _navMeshAgent.SetDestination(_targetTransform.position);
                
                // draw path 
                StartCoroutine(makePathCoroutine());

                break;

            case BossState.normalAttack:
                // normal attack animation
            

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
                    _currentState = BossState.chasing;
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
                StartCoroutine(makePathCoroutine());

                break;

            case BossState.waveBlast:
                lookAtPlayer();

                break;


            case BossState.wideAttack:

                break;

            case BossState.dead:

                break;
        }
    }




    //    idle,
    //    chasing,
    //    normalAttack,
    //    readyDashAttack,
    //    startDashAttack,
    //    onDashAttack,
    //    waveBlast,
    //    wideAttack,
    //    dead
    /*
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

    IEnumerator idle()
    {
        // idle animation 
        _animator.SetBool("Idle", true);


        while (_idleTimeCount >= _idleTime)
        {
            // time delay 
            _idleTimeCount += 0.1f;

            // change state 
            _currentState = BossState.chasing;

            // animation 
            _animator.SetBool("Idle", false);

            yield return new WaitForSeconds(0.1f);
        }
    }


    IEnumerator chasing()
    {
        _navMeshAgent.speed = _monsterSpeed;
        _navMeshAgent.stoppingDistance = _attackRange;

        checkAttackDistance();

        _navMeshAgent.SetDestination(_targetTransform.position);

        // walking animation
        _animator.SetBool("Chase", true);

        // draw path 
        StartCoroutine(makePathCoroutine());

        yield return null;
    }


    IEnumerator normalAttack()
    {
        checkAttackDistance();

        // animation 
        _animator.SetBool("Chase", false);


        // count attack time 
        _attackTimeCount += Time.deltaTime;

        // look at player 
        lookAtPlayer();

        // attack interval 
        if (_attackTimeCount >= _attackInterval)
        {
            _attackTimeCount = 0;
            _attackedCount += 1;

            //if (_attackedCount % 2 == 0)
            //{
            //    _animator.SetBool("AttackRH", true);
            //    yield return StartCoroutine(animationPlay(_animator, AttackRHHash));
            //}
            //else
            //{
            //    _animator.SetBool("AttackLH", true);
            //    yield return StartCoroutine(animationPlay(_animator, AttackLHHash));
            //}

            attackPlayer();
        }

        // normal attack to dash attack 
        if (_attackedCount > _normalAttackCount)
        {
            _animator.SetBool("AttackRH", false);
            _animator.SetBool("AttackLH", false);

            _attackedCount = 0;
            _attackTimeCount = 0;
            _currentState = BossState.readyDashAttack;
        }

        yield return null;
    }

    IEnumerator readyDashAttack()
    {
        lookAtPlayer();

        _dashReadyTimeCount += Time.deltaTime;

        if (_dashReadyTimeCount > _readyDashTime)
        {
            _dashReadyTimeCount = 0;
            _currentState = BossState.startDashAttack;
        }

        yield return null;
    }

    IEnumerator startDashAttack()
    {

         //set dash values
        _navMeshAgent.speed = _dashSpeed;
        _navMeshAgent.isStopped = false;
        _navMeshAgent.stoppingDistance = 2;

        // get player speed and normalize 
        float targetMoveDirLength = _targetCC.velocity.magnitude;

        // set dash direction
        float dashPoint = targetMoveDirLength * _dashPoint;

        // dash in front of player 
        _dashDirection = _targetTransform.position + _targetTransform.forward * dashPoint;

        // add dash count 
        _dashed++;

        // set current state 
        _currentState = BossState.onDashAttack;

        // if boss dashed more than dash count -> change state 
        if (_dashed > _dashCount)
        {
            _dashed = 0;

            _navMeshAgent.speed = _monsterSpeed;
            _currentState = BossState.chasing;
        }

        yield return null;
    }


    IEnumerator onDashAttack()
    {

        dashAttack(_dashDirection);

        // draw line from boss to dash point 
        StartCoroutine(makePathCoroutine());


        yield return null;
    }

    IEnumerator waveBlast()
    {
        // animation play 

        yield return null;
    }



    IEnumerator dead()
    {

        yield return null;
    }
    */




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


        //// attacking player
        //GameObject bullet = ObjectPoolManager.Inst.BringObject(_monsterBulletPrefab);
        //bullet.transform.position = _attackPoint.position;
        //bullet.GetComponent<TempBullet>().Damage = _attackPower;
        //bullet.transform.LookAt(_targetTransform);

        //Rigidbody bulletRB = bullet.GetComponent<Rigidbody>();
        //bulletRB.velocity = _attackPoint.forward * _attackSpeed;
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
                // turn off the dash animation 
                _animator.SetBool("Dash", false);

                waveBlastStart();

                _navMeshAgent.velocity = Vector3.forward;
                _navMeshAgent.isStopped = true;

                _currentState = BossState.waveBlast;
            }
        }
    }


    public void StartWaveBlastAttack()
    {
        // create wave blast 
        _skillMonsterBlast.StartWaveBlastAttack();
    }


    private void waveBlastStart()
    {
        // play animation
        _animator.SetTrigger("AttackBothPaws");
    }

   
    // when wave blast end => change state
    private void waveBlastEnd()
    {
        _currentState = BossState.readyDashAttack;
    }

    // draw path 
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


    // collision check 
    #region Collision check Code 
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Boss touches {other.name}");

        if (other.tag.Equals("PlayerAttack"))
        {
            // get bullet damage 
            if (other.gameObject.TryGetComponent(out Bullet bullet))
            {
                // apply player attack damage 
                applyDamage(bullet._damage);
            }
            // if bullet doens't have damage 
            else
            {
                applyDamage(1);
            }
        }

        if(_currentState == BossState.onDashAttack)
        {
            if (other.tag.Equals("BossWall"))
            {
                // if touches wall, stop moving 
                _navMeshAgent.velocity = Vector3.zero;
                _navMeshAgent.isStopped = true;
     
                // turn off the dash animation 
                _animator.SetBool("Dash", false);

                waveBlastStart();

                _currentState = BossState.waveBlast;
            }
        }
    }
    #endregion


    // apply damage 
    private void applyDamage(float damage)
    {
        _monsterHP -= damage;

        if(_monsterHP <= 0)
        {
            GameManager.Inst.BossDead();
            _currentState = BossState.dead;
            return;
        }
    }
}
