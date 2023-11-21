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
        [SerializeField] private PlayerData _playerData;
        [SerializeField] private List<SkillSelectButton> _skillLevelUpButtons;
        private Dictionary<string, int> _skillDict;
        //private int _skillCount = 0;
        private int _randomNumber;
        

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
                {"Bomb Attack", _playerAttack.bombLevel},
                { "Dash Distance", _playerAttack.dashLevel}
            };
            //_skillCount = 0;
            var _skillList = new List<string>(_skillDict.Keys);
            _randomNumber = Random.Range(0,_skillList.Count);

            // 스킬 선택하기
            foreach (SkillSelectButton selectButton in _skillLevelUpButtons)
            {

                string _skillName = _skillList[_randomNumber];
                //string _skillName = _skillList[0];
                int _skillLevel = _skillDict[_skillName];
                SkillElement element = selectButton.GetComponentInChildren<SkillElement>();
                element.SetImage(_skillName);
                //element.SetLevel(_playerAttack.basicFireLevel + 1);
                element.SetLevel(_skillLevel + 1);
                selectButton.GetComponent<Button>().onClick.AddListener(() =>
                {
                    selectButton.SelectSkillBox();
                    //선택한 스킬 이름에 따라서 스킬 레벨업
                    switch (_skillName)
                    {
                        case "Basic Fire":
                            if(_skillLevel == _playerAttack.basicFireLevel &&
                               _playerAttack.basicFireLevel < 6)
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
                                if (_skillLevel == _playerAttack.droneLevel &&
                                    _playerAttack.droneLevel < 5)
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
                                if (_skillLevel == _playerAttack.bombLevel &&
                                    _playerAttack.bombLevel < 5)
                                    _playerAttack.bombLevel += 1;
                                break;
                            }

                        case "Dash Distance":
                            if(_skillLevel == _playerAttack.dashLevel &&
                                _playerAttack.dashLevel < 5)
                            {
                                _playerAttack.dashLevel += 1;
                                _playerData.dashSpeed += 2;
                            }
                                
                            break;
                            
                    }
                });
                //_skillCount += 1;
            }
        }
    }
}