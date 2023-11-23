using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectDaNyan.Scripts.UI.StageUI
{
    public class PauseUI : MonoBehaviour
    {
        [SerializeField]
        private List<SkillElement> _skillElements;
        private PlayerBasicAttack _playerBasicAttack;
        private PlayerRandomFieldAttack _playerRandomFieldAttack;
        private PlayerAttack _playerAttack;

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
            if (_skillElements.Count <= 0) return; 
            var count = 0;
            //현재 가지고 있는 스킬 표시
            //기본은 무조건 가지고 있음
            // _skillElements[count].SetImage("basicFire");
            _skillElements[count].SetLevel(_playerAttack.basicFireLevel);
            _skillElements[count].gameObject.SetActive(true);
            count += 1;

            if (_playerAttack.isField == true)
            {
            //     _skillElements[count].SetImage("field");
                _skillElements[count].SetLevel(_playerRandomFieldAttack.randomFieldLevel);
                _skillElements[count].gameObject.SetActive(true);
                count += 1;
            }
        }
    }
}