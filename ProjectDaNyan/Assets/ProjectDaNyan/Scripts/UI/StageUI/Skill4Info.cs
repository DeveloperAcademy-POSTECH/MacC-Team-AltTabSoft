using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill4Info : MonoBehaviour
{
    private GameObject _star1;
    private GameObject _star2;
    private GameObject _star3;
    private GameObject _star4;
    private GameObject _star5;
    private SkillLevel _skillLevel;
    private PlayerAttack _playerAttack;
    
    private void Awake()
    {
        _star1 = GetComponentInChildren<Star1>().gameObject;
        _star2 = GetComponentInChildren<Star2>().gameObject;
        _star3 = GetComponentInChildren<Star3>().gameObject;
        _star4 = GetComponentInChildren<Star4>().gameObject;
        _star5 = GetComponentInChildren<Star5>().gameObject;
        _skillLevel = FindObjectOfType<SkillLevel>();
        _playerAttack = FindObjectOfType<PlayerAttack>();
    }

    private void OnEnable()
    {
        if (_playerAttack.droneLevel >= 1)
        {
            _star1.SetActive(false);
        }
        
        if (_playerAttack.droneLevel >= 2)
        {
            _star2.SetActive(false);
        }
        
        if (_playerAttack.droneLevel >= 3)
        {
            _star3.SetActive(false);
        }
        
        if (_playerAttack.droneLevel >= 4)
        {
            _star4.SetActive(false);
        }
        
        if (_playerAttack.droneLevel >= 5)
        {
            _star5.SetActive(false);
        }

        if (_playerAttack.droneLevel >= 6)
        {
            _skillLevel.gameObject.SetActive(false);
        }
    }
}
