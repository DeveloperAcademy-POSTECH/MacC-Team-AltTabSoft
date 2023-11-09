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
        [SerializeField]
        private List<SkillElement> _skillElements;

        [SerializeField] private List<SkillSelectButton> _skillLevelUpButtons;
        
        private void Awake()
        {
            _playerAttack = FindObjectOfType<PlayerAttack>();
            _playerBasicAttack = FindObjectOfType<PlayerBasicAttack>();
            foreach (SkillElement skillElement in _skillElements)
            {
                skillElement.gameObject.SetActive(false);
            }
        }

        private void OnEnable()
        {
            var count = 0;
            //현재 가지고 있는 스킬 표시
            //기본은 무조건 가지고 있음
            _skillElements[count].SetImage("basicFire");
            _skillElements[count].SetLevel(_playerBasicAttack.basicFireLevel);
            _skillElements[count].gameObject.SetActive(true);
            count += 1;

            if (_playerAttack.isField == true)
            {
                _skillElements[count].SetImage("field");
                _skillElements[count].SetLevel(_playerRandomFieldAttack.randomFieldLevel);
                _skillElements[count].gameObject.SetActive(true);
                count += 1;
            }
            
            // 스킬 선택하기
            foreach (SkillSelectButton selectButton in _skillLevelUpButtons)
            {
                //일단 베이직 only
                SkillElement element = selectButton.GetComponentInChildren<SkillElement>();
                element.SetImage("basicFire");
                element.SetLevel(_playerBasicAttack.basicFireLevel + 1);
                selectButton.GetComponent<Button>().onClick.AddListener(() =>
                {
                    selectButton.SelectSkillBox();
                    _playerBasicAttack.basicFireLevel += 1;
                });
            }
        }
    }
}