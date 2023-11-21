using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

public class PlayerDashCountText : MonoBehaviour
{
    private TextMeshProUGUI _text;
    private PlayerStatus _playerStatus;
    
    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
        _playerStatus = FindObjectOfType<PlayerStatus>();
    }
    
    private void LateUpdate()
    {
        var count = String.Format("{0:D2}", _playerStatus.DashCharged);
        _text.text = string.Concat("DASH", count);
    }
    
}
