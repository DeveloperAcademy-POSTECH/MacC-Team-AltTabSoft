using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : JoystickController
{
    [SerializeField] private GameObject playerObject;
    [SerializeField] private int playerSpeed = 500;

    private Rigidbody playerRigid;
    private TrailRenderer playerTrailRenderer;
    private Animator playerAnim;

    private Vector3 movePosition;
    private Vector3 dashMovePosition;

    private float movementSpeed = 50f;
    private float jumpForce = 300;
    private float timeBeforeNextJump = 1.2f;
    private float canJump = 0f;

    private float playerRotationPositionX;
    private float playerRotationPositionY;

    private PlayerState playerState = PlayerState.stop;
    
    protected override void Start()
    {
        playerRigid = playerObject.GetComponent<Rigidbody>();
        playerTrailRenderer = playerObject.GetComponent<TrailRenderer>();
        playerAnim = playerObject.GetComponent<Animator>();

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

    public override void OnPointerUp(PointerEventData eventData)
    {
        background.gameObject.SetActive(false);
        base.OnPointerUp(eventData);
        playerState = PlayerState.stop;
    }

    void Update()
    {

    }

    private void FixedUpdate()
    {
        PlayerRotate();
        PlayerMove();
    }

    void PlayerRotate()
    {
        if (playerState != PlayerState.stop)
        {
            playerRotationPositionX = this.Direction.x - this.Direction.y;
            playerRotationPositionY = this.Direction.y + this.Direction.x;
        }

        playerRigid.MoveRotation(Quaternion.Euler(0f,
            Mathf.Atan2(playerRotationPositionX, playerRotationPositionY) * Mathf.Rad2Deg, 0f));
    }
    void PlayerMove()
    {
        Vector3 normalized = new Vector3(this.Direction.x+this.Direction.y, 0, this.Direction.y-this.Direction.x).normalized;
        movePosition = normalized * (Time.deltaTime * playerSpeed);
        playerRigid.velocity = movePosition;
        dashMovePosition = movePosition;
    }
    private enum PlayerState {walk,dash,stop}
}
