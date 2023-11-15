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
        
        private void Awake()
        {
            _playerAttack = FindObjectOfType<PlayerAttack>();
            _playerBasicAttack = FindObjectOfType<PlayerBasicAttack>();
        }

        private void OnEnable()
        {
            // 스킬 선택하기
            foreach (SkillSelectButton selectButton in _skillLevelUpButtons)
            {
                //일단 베이직 only
                SkillElement element = selectButton.GetComponentInChildren<SkillElement>();
                element.SetImage("basicFire");
                element.SetLevel(_playerAttack.basicFireLevel + 1);
                selectButton.GetComponent<Button>().onClick.AddListener(() =>
                {
                    selectButton.SelectSkillBox();
                    _playerAttack.basicFireLevel += 1;
                });
            }
        }
    }
}