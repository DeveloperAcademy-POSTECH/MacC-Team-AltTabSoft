using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private GameObject playerObject;
    [SerializeField] private GameObject playerMoveDirectionObject;
    [SerializeField] private GameObject playerDashDirectionObject;
    
    [SerializeField] private int playerSpeed = 10;
    [SerializeField] private int dashSpeed = 10;
    [SerializeField] private int dashLimitTic1SecondsTo50 = 10; //50틱 = 1초, 대시가 지속될 시간 설정

    [SerializeField] private JoystickController _joy;
    
    private Rigidbody playerRigid;
    private TrailRenderer playerTrailRenderer;
    private Animator playerAnim;
    private CharacterController playerCharacterController;

    private Vector3 movePosition;
    private Vector3 dashMovePosition;
    
    private int dashTimerCount = 0;

    private float movementSpeed = 50f;
    private float jumpForce = 300;
    private float timeBeforeNextJump = 1.2f;
    private float canJump = 0f;

    private float playerRotationPositionX;
    private float playerRotationPositionY;
    private float _floatingPosition;

    void Start()
    {
        playerTrailRenderer = playerObject.GetComponent<TrailRenderer>();
        playerAnim = playerObject.GetComponent<Animator>();
        playerCharacterController = playerObject.GetComponent<CharacterController>();

        // 게임이 시작될 때 캐릭터의 머리가 카메라를 바라보도록 회전각을 설정
        playerRotationPositionX = 1;
        playerRotationPositionY = -1;

        _joy = GameObject.FindGameObjectWithTag("JoyStick").gameObject.GetComponent<JoystickController>();
    }

    private void FixedUpdate()
    {
        PlayerRotate();
        PlayerFall();
        
        switch (_joy.playerState)
        {
            case JoystickController.PlayerState.walk:
            {
                PlayerWalk();
                break;
            }
            case JoystickController.PlayerState.dash:
            {
                PlayerDash();
                break;
            }
            case JoystickController.PlayerState.onTheRock:
                break;
            default:
                PlayerStop();
                break;
        }
    }

    void PlayerRotate()
    {
        transform.LookAt(transform.position+movePosition+dashMovePosition);
    }

    void PlayerStop()
    {
        playerCharacterController.Move(new Vector3(0, _floatingPosition, 0));
        
        //이동방향 및 대시방향 표시하는 오브젝트 끄기
        playerMoveDirectionObject.SetActive(false);
        playerDashDirectionObject.SetActive(false);
    }
    void PlayerWalk()
    {
        Vector3 normalized = new Vector3(_joy.Horizontal+_joy.Vertical, 0, _joy.Vertical-_joy.Horizontal).normalized;
        movePosition = normalized * (Time.deltaTime * playerSpeed);
        playerCharacterController.Move(new Vector3(movePosition.x,0,movePosition.z));
        dashMovePosition = movePosition * dashSpeed;
        //대시 준비 상태가 아니라면 현재 이동방향을 표시
        if (!_joy.isJoystickPositionGoEnd)
        {
            playerMoveDirectionObject.SetActive(true);
            playerDashDirectionObject.SetActive(false);
        }
        else
        {
            playerMoveDirectionObject.SetActive(false);
            playerDashDirectionObject.SetActive(true);
        }
    }

    void PlayerDash()
    {
            playerCharacterController.Move(new Vector3(dashMovePosition.x,_floatingPosition,dashMovePosition.z));
            dashTimerCount += 1;
            
            if (dashTimerCount == dashLimitTic1SecondsTo50)
            {
                dashTimerCount = 0;
                _joy.playerState = JoystickController.PlayerState.stop;
            }
            playerDashDirectionObject.SetActive(false);
    }

    void PlayerFall()
    {
        if (playerCharacterController.isGrounded == false)
        {
            _floatingPosition += -9.81f * Time.deltaTime;
        }
        else
        {
            _floatingPosition = 0f;
        }
    }

    void playerWallReflection(Collision wall)
    {
        dashTimerCount = 0;
        Vector3 normal = wall.contacts[0].normal; //법선벡터
        playerCharacterController.Move(new Vector3(-dashMovePosition.x,0,-dashMovePosition.z));
        dashMovePosition = Vector3.Reflect(dashMovePosition, normal);
    }

    void playerMoveOnTheRock(Collision rock)
    {
        _joy.playerState = JoystickController.PlayerState.onTheRock;
        _floatingPosition = rock.transform.position.y + 5.1f;
        transform.position = (new Vector3(rock.transform.position.x,rock.transform.position.y,rock.transform.position.z));
    }

    //충돌 시 뚫고 나갈 수 없는 물체에 닿았을 때 작동 (벽, 돌 등)
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Wall")
        {
            Debug.Log("Wall");
            if (_joy.playerState == JoystickController.PlayerState.dash)
            {
                playerWallReflection(other);
            }
        }
        else if (other.gameObject.tag == "Rock")
        {
            Debug.Log("Rock");
            if (_joy.playerState != JoystickController.PlayerState.onTheRock)
            {
                if (_joy.playerState == JoystickController.PlayerState.dash)
                {
                    playerMoveOnTheRock(other);
                }
            }
        }
    }
    
    //충돌 시 뚫고 나갈 수 없는 물체에 닿은 상태를 유지할 때 작동 (벽, 돌 등)
    //이게 없으면 캐릭터가 벽이랑 붙은 상태에서 벽으로 대시할 때 대시 판정이 발생하지 않음
    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.tag == "Wall")
        {
            Debug.Log("In Wall");
            if (_joy.playerState == JoystickController.PlayerState.dash)
            {
                dashTimerCount = 0;
                playerCharacterController.Move(new Vector3(-dashMovePosition.x,0,-dashMovePosition.z));
                dashMovePosition = new Vector3(-dashMovePosition.x, 0, -dashMovePosition.z);
            }
        }
        else if (other.gameObject.tag == "Rock")
        {
            if (_joy.playerState != JoystickController.PlayerState.onTheRock)
            {
                if (_joy.playerState == JoystickController.PlayerState.dash)
                {
                    
                }
            }
        }
    }
}