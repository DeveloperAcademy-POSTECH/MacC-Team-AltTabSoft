using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace ProjectDaNyan.Scripts.UI.StageUI
{
    public class SkillSelectUI : MonoBehaviour
    {
        private PlayerAttack _playerAttack;
        private PlayerBasicAttack _playerBasicAttack;
        private PlayerRandomFieldAttack _playerRandomFieldAttack;
        [SerializeField] private List<SkillSelectButton> _skillLevelUpButtons;
        private Dictionary<string, int> _skillDict;
        private int _skillCount = 0;

        private void Awake()
        {
            _playerAttack = FindObjectOfType<PlayerAttack>();
            _playerBasicAttack = FindObjectOfType<PlayerBasicAttack>();
            
        }

        private void OnEnable()
        {
            _skillDict = new Dictionary<string, int>()
            {
                {"Basic Fire", _playerAttack.basicFireLevel},
                {"Drone Attack", _playerAttack.droneLevel},
                {"Bomb Attack", _playerAttack.bombLevel}
            };
            _skillCount = 0;
            var _skillList = new List<string>(_skillDict.Keys);

            // 스킬 선택하기
            foreach (SkillSelectButton selectButton in _skillLevelUpButtons)
            {
                
                string _skillName = _skillList[_skillCount];
                int _skillLevel = _skillDict[_skillName];
                SkillElement element = selectButton.GetComponentInChildren<SkillElement>();
                element.SetImage(_skillName);
                //element.SetLevel(_playerAttack.basicFireLevel + 1);
                element.SetLevel(_skillLevel + 1);
                selectButton.GetComponent<Button>().onClick.AddListener(() =>
                {
                    selectButton.SelectSkillBox();
                    //_playerAttack.basicFireLevel += 1;
                    switch (_skillName)
                    {
                        case "Basic Fire":
                            if(_skillLevel == _playerAttack.basicFireLevel)
                            _playerAttack.basicFireLevel += 1;
                            break;
                        case "Drone Attack":
                            if (_playerAttack.isDrone == false)
                            {
                                if(_skillLevel == _playerAttack.droneLevel)
                                {
                                    _playerAttack.isDrone = true;
                                    _playerAttack.droneLevel += 1;
                                }
                                
                                break;
                            }
                            else
                            {
                                if (_skillLevel == _playerAttack.droneLevel)
                                    _playerAttack.droneLevel += 1;
                                break;
                            }
                        case "Bomb Attack":
                            if(_playerAttack.isBomb == false)
                            {
                                if(_skillLevel == _playerAttack.bombLevel)
                                {
                                    _playerAttack.isBomb = true;
                                    _playerAttack.bombLevel += 1;
                                }
                                break;
                            }
                            else
                            {
                                if (_skillLevel == _playerAttack.bombLevel)
                                    _playerAttack.bombLevel += 1;
                                break;
                            }
                            
                    }
                });
                _skillCount += 1;
            }
        }
    }
}