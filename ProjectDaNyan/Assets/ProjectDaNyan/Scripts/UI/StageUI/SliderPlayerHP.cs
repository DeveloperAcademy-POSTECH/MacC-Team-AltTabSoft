using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderPlayerHP : MonoBehaviour
{
    private PlayerStatus _playerStatus;
    private Slider _playerHpSlider; 
    void Awake()
    {
        _playerStatus = FindObjectOfType<PlayerStatus>();
        _playerHpSlider = GetComponent<Slider>();
    }
    // Update is called once per frame
    void Update()
    {
        _playerHpSlider.value = (float)_playerStatus.Player_Now_HP / _playerStatus.Player_Max_HP;
    }
}
