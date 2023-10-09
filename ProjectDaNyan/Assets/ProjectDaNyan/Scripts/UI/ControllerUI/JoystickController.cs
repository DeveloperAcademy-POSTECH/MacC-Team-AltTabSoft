using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoystickController : FloatingJoystick
{
    public enum PlayerState {walk,dash,stop}
    public PlayerState playerState = PlayerState.stop;
    protected internal bool isJoystickPositionGoEnd = false;
    
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
    
    public override void OnPointerDown(PointerEventData eventData)
    {
        playerState = PlayerState.walk;
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