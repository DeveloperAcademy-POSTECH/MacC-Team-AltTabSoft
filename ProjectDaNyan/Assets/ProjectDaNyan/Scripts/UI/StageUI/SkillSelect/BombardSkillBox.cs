using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombardSkillBox : MonoBehaviour
{
    private GameObject _newMark;
    private GameObject _star2;
    private GameObject _star3;
    private GameObject _star4;
    private GameObject _star5;
    private PlayerAttack _playerAttack;

    private void Awake()
    {
        _newMark = GetComponentInChildren<NewMarkText>().gameObject;
        _star2 = GetComponentInChildren<Star2>().gameObject;
        _star3 = GetComponentInChildren<Star3>().gameObject;
        _star4 = GetComponentInChildren<Star4>().gameObject;
        _star5 = GetComponentInChildren<Star5>().gameObject;
        _playerAttack = FindObjectOfType<PlayerAttack>();
    }

    public void SetSkillBoxUI()
    {
        SetNewMark();
        SetLevelStar();
    }

    private void SetNewMark()
    {
        if (_playerAttack.bombLevel != 1)
        {
            _newMark.SetActive(false);
        }
    }

    private void SetLevelStar()
    {
        if (_playerAttack.bombLevel >= 2)
        {
            _star2.SetActive(false);
        }
        
        if (_playerAttack.bombLevel >= 3)
        {
            _star3.SetActive(false);
        }
        
        if (_playerAttack.bombLevel >= 4)
        {
            _star4.SetActive(false);
        }
        
        if (_playerAttack.bombLevel >= 5)
        {
            _star5.SetActive(false);
        }
    }
}