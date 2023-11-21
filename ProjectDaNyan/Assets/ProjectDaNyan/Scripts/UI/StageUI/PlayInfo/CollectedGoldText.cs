using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class CollectedGoldText : MonoBehaviour
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
        _text.text = String.Format("{0:D2}", _playerStatus.Player_collected_gold);
    }
}
