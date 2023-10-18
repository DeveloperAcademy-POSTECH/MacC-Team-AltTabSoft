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
        Debug.Log("왜 안됨");
        _playerStatus.Player_now_EXP = 0;
        skillUI.SetActive(false);
        GameManager.Inst.ResumeGame();
        
    }
}
