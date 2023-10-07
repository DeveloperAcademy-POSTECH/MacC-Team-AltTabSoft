using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : JoystickController
{
    [SerializeField] private GameObject playerObject;
    [SerializeField] private GameObject playerMoveDirectionObject;
    [SerializeField] private GameObject playerDashDirectionObject;
    
    [SerializeField] private int playerSpeed = 50;
    [SerializeField] private int dashSpeed = 10;
    [SerializeField] private int dashLimitTic1SecondsTo50 = 50; //50틱 = 1초, 대시가 지속될 시간 설정
    
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

    private PlayerState playerState = PlayerState.stop;

    private bool isJoystickPositionGoEnd = false;
    
    protected override void Start()
    {
        playerRigid = playerObject.GetComponent<Rigidbody>();
        playerTrailRenderer = playerObject.GetComponent<TrailRenderer>();
        playerAnim = playerObject.GetComponent<Animator>();
        playerCharacterController = playerObject.GetComponent<CharacterController>();

        // 게임이 시작될 때 캐릭터의 머리가 카메라를 바라보도록 회전각을 설정
        playerRotationPositionX = 1;
        playerRotationPositionY = -1;
        
        base.Start();
        background.gameObject.SetActive(false);
    }
    
    public override void OnPointerDown(PointerEventData eventData)
    {
        playerState = PlayerState.walk;
        background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
        background.gameObject.SetActive(true);
        base.OnPointerDown(eventData);
    }
    
    protected override void HandleInput(float magnitude, Vector2 normalised, Vector2 radius, Camera cam)
    {
        if (magnitude > deadZone)
        {
            if (magnitude > 1)
            {
                input = normalised;
                isJoystickPositionGoEnd = true;
                
                //대시 준비 시 대시 방향 표시
                playerMoveDirectionObject.SetActive(false);
                playerDashDirectionObject.SetActive(true);
            }
            else
            {
                isJoystickPositionGoEnd = false;
            }
        }
        else
        {
            input = Vector2.zero;
        }
        
        Debug.Log(isJoystickPositionGoEnd);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        background.gameObject.SetActive(false);
        base.OnPointerUp(eventData);

        if (isJoystickPositionGoEnd)
        {
            playerState = PlayerState.dash;
            isJoystickPositionGoEnd = false;
        }
        else
        {
            playerState = PlayerState.stop;
        }
    }

    void Update()
    {

    }

    private void FixedUpdate()
    {
        PlayerRotate();

        switch (playerState)
        {
            case PlayerState.walk:
            {
                PlayerWalk();
                break;
            }
            case PlayerState.dash:
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
        if (playerState == PlayerState.walk)
        {
            playerRotationPositionX = this.Direction.x - this.Direction.y;
            playerRotationPositionY = this.Direction.y + this.Direction.x;
        }

        playerRigid.MoveRotation(Quaternion.Euler(0f,
            Mathf.Atan2(playerRotationPositionX, playerRotationPositionY) * Mathf.Rad2Deg, 0f));
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
        Vector3 normalized = new Vector3(this.Direction.x+this.Direction.y, 0, this.Direction.y-this.Direction.x).normalized;
        movePosition = normalized * (Time.deltaTime * playerSpeed);
        playerCharacterController.Move(movePosition);
        dashMovePosition = movePosition * dashSpeed;

        //대시 준비 상태가 아니라면 현재 이동방향을 표시
        if (!isJoystickPositionGoEnd)
        {
            playerMoveDirectionObject.SetActive(true);
            playerDashDirectionObject.SetActive(false);
        }
    }

    void PlayerDash()
    {
            playerCharacterController.Move(dashMovePosition);
            dashTimerCount += 1;

            if (dashTimerCount == dashLimitTic1SecondsTo50)
            {
                dashTimerCount = 0;
                playerState = PlayerState.stop;
            }
            
            playerDashDirectionObject.SetActive(false);
    }
    private enum PlayerState {walk,dash,stop}
}
