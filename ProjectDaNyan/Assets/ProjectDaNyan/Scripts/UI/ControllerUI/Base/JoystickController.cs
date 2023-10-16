using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class JoystickController : FloatingJoystick
{
    protected internal bool isJoystickPositionGoEnd = false;
    public PlayerState _playerState;
    
    public override void OnPointerUp(PointerEventData eventData)
    {
        background.gameObject.SetActive(false);
        base.OnPointerUp(eventData);

        if (isJoystickPositionGoEnd)
        {
            if (_playerState.getPsData() == PlayerState.PSData.onTheRock)
            {
                _playerState.setPsData(PlayerState.PSData.exitDashFromRock);
            }
            else
            {
                _playerState.setPsData(PlayerState.PSData.dash);
            }
            isJoystickPositionGoEnd = false;
        }
        else
        {
            _playerState.setPsData(PlayerState.PSData.stop);
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