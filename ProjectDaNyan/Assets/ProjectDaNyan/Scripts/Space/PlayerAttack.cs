using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public bool isLaser = true; //레이저 공격을 입수했는지 여부 
    public bool isUpgrade = true; //초월 여부

    EnemyScanner scanner; //가까운 적 탐지 스크립트
    PlayerBasicAttack playerBasicAttack;
    PlayerLaserAttack playerLaserAttack;

    private void Awake()
    {
        scanner = GetComponent<EnemyScanner>();
        playerBasicAttack = GetComponent<PlayerBasicAttack>();
        playerLaserAttack = GetComponent<PlayerLaserAttack>();
    }

    private void Update()
    {
        AllAttack();
    }

    public void AllAttack()
    {
        //기본공격, 초월공격 활성화 코드
        playerBasicAttack.UseBasicAttack(isUpgrade);
        //관통공격활성화코드
        playerLaserAttack.UseLaserAttack(isLaser);
        
    }
}
