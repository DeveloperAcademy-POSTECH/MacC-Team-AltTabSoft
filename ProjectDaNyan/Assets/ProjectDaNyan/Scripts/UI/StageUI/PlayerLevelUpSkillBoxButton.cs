using System;
using System.Collections;
using System.Collections.Generic;
using ProjectDaNyan.Views.StageUI;
using UnityEngine;

public class PlayerLevelupSkillBoxButton : MonoBehaviour
{
    public GameObject skillUI;
    private PlayerStatus _playerStatus;

    private void Awake()
    {
        _playerStatus = FindObjectOfType<PlayerStatus>();
    }
    public void SelectSkillBox()
    {
        var overFlowedExp = _playerStatus.Player_now_EXP - _playerStatus.Level_Up_Require_EXP ;
        _playerStatus.Player_now_EXP = overFlowedExp; // 레벨에 필요한 만큼만 경험치를 제외, 연속적인 레벨업 가능
        skillUI.SetActive(false);
        GameManager.Inst.ResumeGame();
        
    }
}
