using System;
using System.Collections;
using System.Collections.Generic;
using ProjectDaNyan.Scripts.UI.StageUI;
using UnityEngine;
using UnityEngine.UI;

public class SkillInfoUI : MonoBehaviour
{
    [SerializeField] private DefaultSkillBox defaultSkillBox;
    [SerializeField] private BombardSkillBox bombardSkillBox;
    [SerializeField] private DashSkillBox dashSkillBox;
    [SerializeField] private DroneAttackSkillBox droneAttackSkillBox;
    [SerializeField] private SuperDefaultSkillBox superDefaultSkillBox;
    [SerializeField] private SuperBombardSkillBox superBombardSkillBox;
    [SerializeField] private SuperDashSkillBox superDashSkillBox;
    [SerializeField] private SuperDroneAttackSkillBox superDroneAttackSkillBox;
    
    private SkillInfoUI _skillInfoUI;
    private PlayerAttack _playerAttack;
    private GameObject _pauseUI;
    public Dictionary<string, int> skillDict;

    private void Awake()
    {
        _skillInfoUI = FindObjectOfType<SkillInfoUI>(includeInactive: true);
        _playerAttack = FindObjectOfType<PlayerAttack>();
        _pauseUI = transform.parent.Find("PauseUI").gameObject;
    }

    private void OnEnable()
    {
        skillDict = new Dictionary<string, int>()
        {
            { "Basic Fire", _playerAttack.basicFireLevel },
            { "Drone Attack", _playerAttack.droneLevel },
            { "Bomb Attack", _playerAttack.bombLevel },
            { "Dash Distance", _playerAttack.dashLevel }
        };
        
        var _skillList = new List<string>(skillDict.Keys);

        foreach (var skill in _skillList)
        {
            switch (skill)
            {
                case "Basic Fire":
                    if (_playerAttack.basicFireLevel >= 2 && _playerAttack.basicFireLevel < 6)
                    {
                        defaultSkillBox.gameObject.SetActive(true);
                        defaultSkillBox.SetSkillBoxUI();
                    }
                    else if (_playerAttack.basicFireLevel >= 6)
                    {
                        defaultSkillBox.gameObject.SetActive(false);
                        superDefaultSkillBox.gameObject.SetActive(true);
                    }

                    break;

                case "Drone Attack":
                    if (_playerAttack.droneLevel >= 1 && _playerAttack.droneLevel < 6)
                    {
                        droneAttackSkillBox.gameObject.SetActive(true);
                        droneAttackSkillBox.SetSkillBoxUI();
                    }
                    else if (_playerAttack.droneLevel >= 6)
                    {
                        droneAttackSkillBox.gameObject.SetActive(false);
                        superDroneAttackSkillBox.gameObject.SetActive(true);
                    }

                    break;
                
                case "Bomb Attack":
                    if (_playerAttack.bombLevel >= 1 && _playerAttack.bombLevel < 6)
                    {
                        bombardSkillBox.gameObject.SetActive(true);
                        bombardSkillBox.SetSkillBoxUI();
                    }
                    else if (_playerAttack.bombLevel >= 6)
                    {
                        bombardSkillBox.gameObject.SetActive(false);
                        superBombardSkillBox.gameObject.SetActive(true);
                    }

                    break;
                
                case "Dash Distance":
                    if (_playerAttack.dashLevel >= 2 && _playerAttack.dashLevel < 6)
                    {
                        dashSkillBox.gameObject.SetActive(true);
                        dashSkillBox.SetSkillBoxUI();
                    }
                    else if (_playerAttack.dashLevel >= 6)
                    {
                        dashSkillBox.gameObject.SetActive(false);
                        superDashSkillBox.gameObject.SetActive(true);
                    }

                    break;
            }
        }
    }

    public void GoToPauseUI()
    {
        _skillInfoUI.gameObject.SetActive(false);
        _pauseUI.SetActive(true);
    }
}
