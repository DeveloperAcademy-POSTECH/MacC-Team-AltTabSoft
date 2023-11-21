using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class PlayerLevelText : MonoBehaviour
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
        var level = String.Format("{0:D2}", _playerStatus.player_Level);
        _text.text = string.Concat("LEVEL", level);
    }
}
