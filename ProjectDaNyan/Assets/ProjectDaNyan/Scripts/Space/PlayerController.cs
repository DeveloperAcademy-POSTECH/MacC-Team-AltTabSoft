using System;
using System.Collections;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private GameObject playerObject;
    [SerializeField] private GameObject playerBodyBullet;
    
    [SerializeField] private PlayerData _playerData;
    
    [SerializeField] private JoystickController _joy;
    [SerializeField] private PlayerState _playerState;
    
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

    private PlayerAttack _playerAttack;

    private Vector3 movePosition;
    private Vector3 dashMovePosition;
    
    private int dashTimerCount = 0;
    private int rockQuitTimerCount = 0;

    private float movementSpeed = 50f;
    private float jumpForce = 300;
    private float timeBeforeNextJump = 1.2f;
    private float canJump = 0f;
    private float rockHeight = 4.0f;

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

        //Player attack script for dash level
        _playerAttack = FindObjectOfType<PlayerAttack>();
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
            case PlayerState.PSData.dashStart:
            {
                PlayerDashStart();
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

    void PlayerDashSetting()
    {
        PlayDashSound();
        this.gameObject.layer = 7;
        dashTimerCount = 0;
        playerAnim.SetInteger("State",2);
        if (_playerAttack.dashLevel > 4)
        {
            playerBodyBullet.SetActive(true);
        }
        playerTrailRenderer.emitting = true;
        playerLineRenderer.enabled = false;
    }
    
    void PlayerDashStart()
    {
        PlayerDashSetting();
        _playerState.setPsData(PlayerState.PSData.dash);
    }
    
    void PlayerDash()
    {
        dashTimerCount += 1;
        playerCharacterController.Move(new Vector3(dashMovePosition.x,_floatingPosition,dashMovePosition.z));
        if (dashTimerCount >= _playerData.dashLimitTic1SecondsTo50)
        {
            StartCoroutine(PlayerDashEnds());
        }
    }

    private IEnumerator PlayerDashEnds()
    {
        _playerState.setPsData(PlayerState.PSData.stop);
        playerTrailRenderer.emitting = false;
        playerAnim.SetInteger("State",0);
        playerBodyBullet.SetActive(false);
        
        yield return new WaitForSeconds(0.02f); //대시가 끝나도 추가 1틱 무적시간 (유체화 속성) 유지
        this.gameObject.layer = 6;
    }

    void PlayerFall()
    {
        if (playerCharacterController.isGrounded == false)
        {
            if (this.transform.position.y > 0f)
            { 
                _floatingPosition += -9.81f * Time.deltaTime; 
            }
        }
        else
        {
            _floatingPosition = 0f;
        }
    }

    void PlayerMovingControllOnTheRock()
    {
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
            return;
        }
        rockQuitTimerCount += 1;
    }

    void PlayerMoveOnTheRock(GameObject rock)
    {
        playerAnim.SetInteger("State",0);
        _playerState.setPsData(PlayerState.PSData.onTheRock);
        dashTimerCount = 0;
        transform.position = (new Vector3(rock.transform.position.x,rock.transform.position.y + rockHeight,rock.transform.position.z));
        _soundEffectController.playStageSoundEffect(0.5f,SoundEffectController.StageSoundTypes.Player_Object_Dash);
        playerBodyBullet.SetActive(true);
    }
    
    void PlayerExitStartFromRock()
    {
        PlayerDashSetting();
        transform.position = new Vector3(transform.position.x, y:1.3f, transform.position.z);
        _playerState.setPsData(PlayerState.PSData.exitDashFromRock);
    }
    
    void PlayerExitDashFromRock()
    {
        playerCharacterController.Move(new Vector3(dashMovePosition.x,_floatingPosition,dashMovePosition.z));
        
        //대시가 1/2 진행된 지점에서 바위에서 탈출하는 대시 > 일반 대시로 판정을 변경 > 추후 미세조정 필요
        if (dashTimerCount >= (_playerData.dashLimitTic1SecondsTo50/2))
        {
            _playerState.setPsData(PlayerState.PSData.dash);
        }
        
        dashTimerCount += 1;
    }
    
    public void PlayDashSound()
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

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (_playerState.getPsData() == PlayerState.PSData.dash)
        {
            if (hit.gameObject.CompareTag("Wall"))
            {
                dashMovePosition = Vector3.Reflect(dashMovePosition, hit.normal);
                PlayerDashStart();
            }
            else if (hit.gameObject.CompareTag("Rock"))
            {
                PlayerMoveOnTheRock(hit.gameObject);
            }
            else if (hit.gameObject.layer == 11 || hit.gameObject.layer == 8)
            {
                return;
            }
            else
            {
                PlayerDashEnds();
                dashTimerCount -= 1;
            }
        }
    }
}