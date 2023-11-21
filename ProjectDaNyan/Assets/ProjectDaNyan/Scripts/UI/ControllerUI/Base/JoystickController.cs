using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class JoystickController : FloatingJoystick
{
    protected internal bool isJoystickPositionGoEnd = false;

    private GameObject player;
    private PlayerState _playerState;
    private PlayerStatus _playerStatus;
    private PlayerController _playerController;
    private SoundEffectController _soundEffectController;

    private bool dashEffectToggle = true;

    protected override void Start()
    {
        base.Start();
        player = GameObject.FindGameObjectWithTag("Player").gameObject;
        _playerState = player.GetComponent<PlayerState>();
        _playerStatus = player.GetComponent<PlayerStatus>();
        _playerController = player.GetComponent<PlayerController>();
        _soundEffectController = player.GetComponent<SoundEffectController>();
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
                PlayDashSound();
            }
            else
            {
                if (_playerStatus.DashCharged > 0)
                {
                    player.gameObject.layer = 7;
                    _playerState.setPsData(PlayerState.PSData.dash);
                    _playerStatus.DashCharged -= 1;
                    PlayDashSound();
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
}