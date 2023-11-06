using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class JoystickController : FloatingJoystick
{
    protected internal bool isJoystickPositionGoEnd = false;
    
    private PlayerState _playerState;
    private PlayerStatus _playerStatus;

    protected override void Start()
    {
        base.Start();
        GameObject player = GameObject.FindGameObjectWithTag("Player").gameObject;
        _playerState = player.GetComponent<PlayerState>();
        _playerStatus = player.GetComponent<PlayerStatus>();
    }
    
    public override void OnPointerUp(PointerEventData eventData)
    {
        background.gameObject.SetActive(false);
        base.OnPointerUp(eventData);

        if (isJoystickPositionGoEnd)
        {
            if (_playerState.getPsData() == PlayerState.PSData.onTheRock)
            {
                _playerState.setPsData(PlayerState.PSData.exitStartFromRock);
            }
            else
            {
                if (_playerStatus.DashCharged > 0)
                {
                    _playerState.setPsData(PlayerState.PSData.dash);
                    _playerStatus.DashCharged -= 1;
                }
                else
                {
                    _playerState.setPsData(PlayerState.PSData.stop);
                }
            }
            isJoystickPositionGoEnd = false;
        }
        else
        {
            if (_playerState.getPsData() != PlayerState.PSData.onTheRock)
            {
                _playerState.setPsData(PlayerState.PSData.stop);
            }
        }
    }
    
    public override void OnPointerDown(PointerEventData eventData)
    {
        if (_playerState.getPsData() != PlayerState.PSData.onTheRock)
        {
            _playerState.setPsData(PlayerState.PSData.walk);
        }
        background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
        background.gameObject.SetActive(true);
        base.OnPointerDown(eventData);
    }
    
    protected override void HandleInput(float magnitude, Vector2 normalised, Vector2 radius, Camera cam)
    {
        base.HandleInput(magnitude, normalised, radius, cam);
        if (magnitude > DeadZone)
        {
            if (magnitude > 1)
            {
                isJoystickPositionGoEnd = true;
            }
            else
            {
                isJoystickPositionGoEnd = false;
            }
        }
    }
}