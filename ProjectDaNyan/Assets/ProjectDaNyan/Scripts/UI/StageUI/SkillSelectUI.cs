using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace ProjectDaNyan.Scripts.UI.StageUI
{
    public class SkillSelectUI : MonoBehaviour
    {
        [SerializeField] private DefaultSkillBox defaultSkillBox;
        [SerializeField] private BombardSkillBox bombardSkillBox;
        [SerializeField] private DashSkillBox dashSkillBox;
        [SerializeField] private DroneAttackSkillBox droneAttackSkillBox;
        [SerializeField] private SuperDefaultSkillBox superDefaultSkillBox;
        [SerializeField] private SuperBombardSkillBox superBombardSkillBox;
        [SerializeField] private SuperDashSkillBox superDashSkillBox;
        [SerializeField] private SuperDroneAttackSkillBox superDroneAttackSkillBox;

        private SkillSelectUI _skillSelectUI;
        private PlayerStatus _playerStatus;
        private PlayerAttack _playerAttack;
        private PlayerBasicAttack _playerBasicAttack;
        private PlayerRandomFieldAttack _playerRandomFieldAttack;
        private SkillSelectButton _skillSelectButton;

        [SerializeField] private PlayerData _playerData;

        public Dictionary<string, int> skillDict;
        public List<string> availableSkills;

        //private int _skillCount = 0;
        private int _randomNumber;
        
        private void Awake()
        {
            _skillSelectUI = FindObjectOfType<SkillSelectUI>(includeInactive: true);
            _playerStatus = FindObjectOfType<PlayerStatus>();
            _playerAttack = FindObjectOfType<PlayerAttack>();
            _playerBasicAttack = FindObjectOfType<PlayerBasicAttack>();
            _skillSelectButton = FindObjectOfType<SkillSelectButton>();
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
            
            //_skillCount = 0;
            var _skillList = new List<string>(skillDict.Keys);
            // 초월에 도달하지 않은 스킬만 필터링
            availableSkills = _skillList.Where(skill => skillDict[skill] < 6).ToList();

            if (availableSkills.Count > 0)
            {
                _randomNumber = Random.Range(0, availableSkills.Count);
                
                // 스킬 선택하기
                string _skillName = availableSkills[_randomNumber];
                int _skillLevel = skillDict[_skillName];

                LevelUpSkill(_skillName, _skillLevel);
            }
            else
            {
                
            }
        }

        private void LevelUpSkill(string skillName, int skillLevel)
        {
            //선택한 스킬 이름에 따라서 스킬 레벨업
            switch (skillName)
            {
                case "Basic Fire":
                    if (skillLevel == _playerAttack.basicFireLevel &&
                        _playerAttack.basicFireLevel < 5)
                    {
                        _playerAttack.basicFireLevel += 1;
                        defaultSkillBox.gameObject.SetActive(true);
                        defaultSkillBox.SetSkillBoxUI();
                    }
                    else if (_playerAttack.basicFireLevel == 5)
                    {
                        _playerAttack.basicFireLevel += 1;
                        superDefaultSkillBox.gameObject.SetActive(true);
                    }

                    break;

                case "Drone Attack":
                    if (_playerAttack.isDrone == false)
                    {
                        if (skillLevel == _playerAttack.droneLevel)
                        {
                            _playerAttack.isDrone = true;
                            _playerAttack.droneLevel += 1;
                            droneAttackSkillBox.gameObject.SetActive(true);
                            droneAttackSkillBox.SetSkillBoxUI();
                        }

                        break;
                    }
                    else
                    {
                        if (skillLevel == _playerAttack.droneLevel &&
                            _playerAttack.droneLevel < 5)
                        {
                            _playerAttack.droneLevel += 1;
                            droneAttackSkillBox.gameObject.SetActive(true);
                            droneAttackSkillBox.SetSkillBoxUI();
                        }
                        else if (_playerAttack.droneLevel == 5)
                        {
                            _playerAttack.droneLevel += 1;
                            superDroneAttackSkillBox.gameObject.SetActive(true);
                        }

                        break;
                    }

                case "Bomb Attack":
                    if (_playerAttack.isBomb == false)
                    {
                        if (skillLevel == _playerAttack.bombLevel)
                        {
                            _playerAttack.isBomb = true;
                            _playerAttack.bombLevel += 1;
                            bombardSkillBox.gameObject.SetActive(true);
                            bombardSkillBox.SetSkillBoxUI();
                        }

                        break;
                    }
                    else
                    {
                        if (skillLevel == _playerAttack.bombLevel &&
                            _playerAttack.bombLevel < 5)
                        {
                            _playerAttack.bombLevel += 1;
                            bombardSkillBox.gameObject.SetActive(true);
                            bombardSkillBox.SetSkillBoxUI();
                        }
                        else if (_playerAttack.bombLevel == 5)
                        {
                            _playerAttack.bombLevel += 1;
                            superBombardSkillBox.gameObject.SetActive(true);
                        }

                        break;
                    }

                case "Dash Distance":
                    if (skillLevel == _playerAttack.dashLevel &&
                        _playerAttack.dashLevel < 5)
                    {
                        _playerAttack.dashLevel += 1;
                        _playerData.dashSpeed += 2;
                        dashSkillBox.gameObject.SetActive(true);
                        dashSkillBox.SetSkillBoxUI();
                    }
                    else if (_playerAttack.dashLevel == 5)
                    {
                        _playerAttack.dashLevel += 1;
                        superDashSkillBox.gameObject.SetActive(true);
                    }

                    break;

            }
        }

        public void SelectSkillBox()
        {
            var overFlowedExp = _playerStatus.Player_now_EXP - _playerStatus.Level_Up_Require_EXP;
            _playerStatus.Player_now_EXP = overFlowedExp; // 레벨에 필요한 만큼만 경험치를 제외, 연속적인 레벨업 가능
            defaultSkillBox.gameObject.SetActive(false);
            droneAttackSkillBox.gameObject.SetActive(false);
            bombardSkillBox.gameObject.SetActive(false);
            dashSkillBox.gameObject.SetActive(false);
            
            superDefaultSkillBox.gameObject.SetActive(false);
            superDroneAttackSkillBox.gameObject.SetActive(false);
            superBombardSkillBox.gameObject.SetActive(false);
            superDashSkillBox.gameObject.SetActive(false);
            
            _skillSelectUI.gameObject.SetActive(false);
            GameManager.Inst.ResumeGame();
        }
    }
}