using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill1Info : MonoBehaviour
{
    private GameObject _star2;
    private GameObject _star3;
    private GameObject _star4;
    private GameObject _star5;
    private SkillLevel _skillLevel;
    private PlayerAttack _playerAttack;
    
    private void Awake()
    {
        _star2 = GetComponentInChildren<Star2>().gameObject;
        _star3 = GetComponentInChildren<Star3>().gameObject;
        _star4 = GetComponentInChildren<Star4>().gameObject;
        _star5 = GetComponentInChildren<Star5>().gameObject;
        _skillLevel = FindObjectOfType<SkillLevel>();
        _playerAttack = FindObjectOfType<PlayerAttack>();
    }

    private void OnEnable()
    {
        if (_playerAttack.basicFireLevel >= 2)
        {
            _star2.SetActive(false);
        }
        
        if (_playerAttack.basicFireLevel >= 3)
        {
            _star3.SetActive(false);
        }
        
        if (_playerAttack.basicFireLevel >= 4)
        {
            _star4.SetActive(false);
        }
        
        if (_playerAttack.basicFireLevel >= 5)
        {
            _star5.SetActive(false);
        }

        if (_playerAttack.basicFireLevel >= 6)
        {
            _skillLevel.gameObject.SetActive(false);
        }
    }
}
