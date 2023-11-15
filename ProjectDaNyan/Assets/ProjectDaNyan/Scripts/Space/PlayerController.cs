using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private GameObject playerObject;
    
    [SerializeField] private PlayerData _playerData;
    
    [SerializeField] private JoystickController _joy;
    [SerializeField] private PlayerState _playerState;

    [SerializeField] private float rockHeight;
    
    [SerializeField] private Animator playerAnim;

    [SerializeField] private SoundEffectController _soundEffectController;
    
    private enum AnimatorStateName
    {
        isStop = 0,
        isWalk = 1,
        isDash = 2,
        isDeath = 3
    }
    
    private Rigidbody playerRigid;
    private TrailRenderer playerTrailRenderer;
    private CharacterController playerCharacterController;
    private LineRenderer playerLineRenderer;

    private Vector3 movePosition;
    private Vector3 dashMovePosition;
    
    private int dashTimerCount = 0;
    private int rockQuitTimerCount = 0;

    private float movementSpeed = 50f;
    private float jumpForce = 300;
    private float timeBeforeNextJump = 1.2f;
    private float canJump = 0f;

    private float playerRotationPositionX;
    private float playerRotationPositionY;
    private float _floatingPosition;

    private bool dashEffectToggle = true;
    private bool isWallReflectDash = false;
    
    void Start()
    {
        playerTrailRenderer = playerObject.GetComponent<TrailRenderer>();
        playerCharacterController = playerObject.GetComponent<CharacterController>();
        playerLineRenderer = playerObject.GetComponent<LineRenderer>();

        // 게임이 시작될 때 캐릭터의 머리가 카메라를 바라보도록 회전각을 설정
        playerRotationPositionX = 1;
        playerRotationPositionY = -1;

        _joy = GameObject.FindGameObjectWithTag("JoyStick").gameObject.GetComponent<JoystickController>();
        }

    private void FixedUpdate()
    {
        PlayerRotate();
        PlayerFall();
        
        switch (_playerState.getPsData())
        {
            case PlayerState.PSData.walk:
            {
                PlayerWalk();
                break;
            }
            case PlayerState.PSData.dash:
            {
                PlayerDash();
                break;
            }
            case PlayerState.PSData.onTheRock:
            {
                PlayerMovingControllOnTheRock();
                break;
            }
            case PlayerState.PSData.exitStartFromRock:
            {
                PlayerExitStartFromRock();
                break;
            }
            case PlayerState.PSData.exitDashFromRock:
            {
                PlayerExitDashFromRock();
                break;
            }
            default:
            {
                PlayerStop();
                break;
            }
        }
    }

    void PlayerRotate()
    {
        transform.LookAt(transform.position+movePosition+dashMovePosition);
    }

    void PlayerStop()
    {
        playerAnim.SetInteger("State",0);
        playerCharacterController.Move(new Vector3(0, _floatingPosition, 0));
        
        //이동방향 및 대시방향 표시하는 오브젝트 끄기
        playerLineRenderer.enabled = false;
    }
    void PlayerWalk()
    {
        playerAnim.SetInteger("State",1);
        Vector3 normalized = new Vector3(_joy.Horizontal+_joy.Vertical, 0, _joy.Vertical-_joy.Horizontal).normalized;
        movePosition = normalized * (Time.deltaTime * _playerData.playerSpeed);
        playerCharacterController.Move(new Vector3(movePosition.x,_floatingPosition,movePosition.z));
        dashMovePosition = movePosition * _playerData.dashSpeed;
        //대시 준비 상태가 아니라면 현재 이동방향을 표시
        if (!_joy.isJoystickPositionGoEnd)
        {
            playerLineRenderer.enabled = true;
            playerLineRenderer.SetPosition(0,this.transform.position);
            playerLineRenderer.SetPosition(1, this.transform.position + movePosition * _playerData.playerSpeed);
        }
        else
        {
            playerLineRenderer.enabled = true;
            playerLineRenderer.SetPosition(0,this.transform.position);
            playerLineRenderer.SetPosition(1, this.transform.position + (dashMovePosition * _playerData.dashLimitTic1SecondsTo50));
        }
    }

    void PlayerDash()
    {
        if (dashTimerCount == 0)
        {
            PlayDashSound();
        }
        playerAnim.SetInteger("State",2);

        playerTrailRenderer.emitting = true;
        playerLineRenderer.enabled = false;
        
        this.gameObject.layer = 7;
        playerCharacterController.Move(new Vector3(dashMovePosition.x,_floatingPosition,dashMovePosition.z));
        dashTimerCount += 1;
            
        if (dashTimerCount >= _playerData.dashLimitTic1SecondsTo50)
        {
            dashTimerCount = 0;
            _playerState.setPsData(PlayerState.PSData.stop);
            this.gameObject.layer = 6;
            playerTrailRenderer.emitting = false;
            playerAnim.SetInteger("State",0);
        }
    }

    void PlayerFall()
    {
        if (playerCharacterController.isGrounded == false && _floatingPosition >= 0f)
        {
            _floatingPosition += -9.81f * Time.deltaTime;
        }
        else
        {
            _floatingPosition = 0f;
        }
    }

    void PlayerMovingControllOnTheRock()
    {
        rockQuitTimerCount += 1;
        Vector3 normalized = new Vector3(_joy.Horizontal+_joy.Vertical, 0, _joy.Vertical-_joy.Horizontal).normalized;

        playerLineRenderer.enabled = true;
        playerLineRenderer.SetPosition(0,this.transform.position);
        playerLineRenderer.SetPosition(1, this.transform.position + (dashMovePosition * _playerData.dashLimitTic1SecondsTo50));

        if (normalized.x != 0 || normalized.z != 0)
        {
            dashMovePosition = normalized * (Time.deltaTime * _playerData.playerSpeed * _playerData.dashSpeed);
        }

        if (rockQuitTimerCount >= _playerData.onTheRockQuitTic)
        {
            rockQuitTimerCount = 0;
            _playerState.setPsData(PlayerState.PSData.exitStartFromRock);
        }
    }

    void PlayerWallReflection(Collision wall)
    {
        dashMovePosition = Vector3.Reflect(dashMovePosition, wall.contacts[0].normal);
        dashTimerCount = 0;
        isWallReflectDash = true;
    }

    void PlayerMoveOnTheRock(Collision rock)
    {
        _playerState.setPsData(PlayerState.PSData.onTheRock);
        dashTimerCount = 0;
        transform.position = (new Vector3(rock.transform.position.x,rock.transform.position.y + rockHeight,rock.transform.position.z));
        _soundEffectController.playStageSoundEffect(0.5f,SoundEffectController.StageSoundTypes.Player_Object_Dash);
    }
    
    void PlayerExitStartFromRock()
    {
        playerAnim.SetInteger("State",2);
        playerLineRenderer.enabled = false;
        transform.position = new Vector3(transform.position.x, y:transform.position.y - rockHeight, transform.position.z);
        PlayDashSound();
        _playerState.setPsData(PlayerState.PSData.exitDashFromRock);
    }
    void PlayerExitDashFromRock()
    {
        playerTrailRenderer.emitting = true;

        this.gameObject.layer = 7;
        playerCharacterController.Move(new Vector3(dashMovePosition.x,_floatingPosition,dashMovePosition.z));
        
        //대시가 1/2 진행된 지점에서 바위에서 탈출하는 대시 > 일반 대시로 판정을 변경 > 추후 미세조정 필요
        if (dashTimerCount >= (_playerData.dashLimitTic1SecondsTo50/2))
        {
            _playerState.setPsData(PlayerState.PSData.dash);
        }
        
        dashTimerCount += 1;
    }

    void PlayDashSound()
    {
        if (!isWallReflectDash)
        {
            if (dashEffectToggle)
            {
                _soundEffectController.playStageSoundEffect(0.5f,SoundEffectController.StageSoundTypes.Player_Dash_0);
            }
            else
            {
                _soundEffectController.playStageSoundEffect(0.5f,SoundEffectController.StageSoundTypes.Player_Dash_1);
            }
            dashEffectToggle = !dashEffectToggle;
        }
        else
        {
            _soundEffectController.playStageSoundEffect(2f,SoundEffectController.StageSoundTypes.Player_Object_Dash);
            isWallReflectDash = false;
        }

    }

    //충돌 시 뚫고 나갈 수 없는 물체에 닿았을 때 작동 (벽, 돌 등)
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            playerAnim.SetInteger("State",0);
            if (_playerState.getPsData() == PlayerState.PSData.dash)
            {
                PlayerWallReflection(other);
                Debug.Log("wallcollide");
            }
        }
        else if (other.gameObject.CompareTag("Rock"))
        {
            playerAnim.SetInteger("State",0);
            if (_playerState.getPsData() == PlayerState.PSData.dash)
            {
                PlayerMoveOnTheRock(other);
            }
        }
    }
    
    //충돌 시 뚫고 나갈 수 없는 물체에 닿은 상태를 유지할 때 작동 (벽, 돌 등)
    //이게 없으면 캐릭터가 벽이랑 붙은 상태에서 벽으로 대시할 때 대시 판정이 발생하지 않을 수 있음
    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.tag == "Wall")
        {
            playerAnim.SetInteger("State",0);
            if (_playerState.getPsData() == PlayerState.PSData.dash)
            {
                PlayerWallReflection(other);
                Debug.Log("wallstay");
            }
        }
        else if (other.gameObject.CompareTag("Rock"))
        {
            playerAnim.SetInteger("State",0);
            Debug.Log(_playerState.getPsData());
            if (_playerState.getPsData() == PlayerState.PSData.dash)
            {
                PlayerMoveOnTheRock(other);
                Debug.Log(_playerState.getPsData());
            }
        }
    }
}