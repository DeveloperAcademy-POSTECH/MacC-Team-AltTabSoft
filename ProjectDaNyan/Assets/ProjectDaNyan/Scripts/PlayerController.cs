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
        playerCharacterController.Move(new Vector3(0, 0, 0));
        
        //이동방향 및 대시방향 표시하는 오브젝트 끄기
        playerMoveDirectionObject.SetActive(false);
        playerDashDirectionObject.SetActive(false);
    }
    void PlayerWalk()
    {
        Vector3 normalized = new Vector3(_joy.Horizontal+_joy.Vertical, 0, _joy.Vertical-_joy.Horizontal).normalized;
        movePosition = normalized * (Time.deltaTime * playerSpeed);
        playerCharacterController.Move(movePosition);
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
            playerCharacterController.Move(dashMovePosition);
            dashTimerCount += 1;
            
            if (dashTimerCount == dashLimitTic1SecondsTo50)
            {
                dashTimerCount = 0;
                _joy.playerState = JoystickController.PlayerState.stop;
            }
            playerDashDirectionObject.SetActive(false);
    }
    
}
