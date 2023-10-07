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
    [SerializeField] private int playerSpeed = 50;
    [SerializeField] private int dashSpeed = 10;
    
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
        playerRigid.velocity = new Vector3(0, 0, 0);
    }
    void PlayerWalk()
    {
        Vector3 normalized = new Vector3(this.Direction.x+this.Direction.y, 0, this.Direction.y-this.Direction.x).normalized;
        movePosition = normalized * (Time.deltaTime * playerSpeed);
        playerCharacterController.Move(movePosition);
        dashMovePosition = movePosition * dashSpeed;
    }

    void PlayerDash()
    {
            playerCharacterController.Move(dashMovePosition);
            dashTimerCount += 1;

            if (dashTimerCount == 25)
            {
                dashTimerCount = 0;
                playerState = PlayerState.stop;
            }
    }
    private enum PlayerState {walk,dash,stop}
}
