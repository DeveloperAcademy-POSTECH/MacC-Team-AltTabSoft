using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public bool isUpgrade = true; //초월 여부
    public bool isLaser = true; //레이저 공격을 입수했는지 여부 
    public bool isDrone = false; //드론 공격 입수했는지 여부
    public bool isField = false; //랜덤 필드 공격 입수했는지 여부

    EnemyScanner scanner; //가까운 적 탐지 스크립트
    EnemyScanner droneScanner; //드론의 적 탐지 스크립트
    PlayerBasicAttack playerBasicAttack;
    PlayerLaserAttack playerLaserAttack;
    PlayerDroneAttack playerDroneAttack;
    PlayerRandomFieldAttack playerRandomFieldAttack;
    

    private void Awake()
    {
        scanner = GetComponent<EnemyScanner>();
        //droneScanner = playerDroneAttack.GetComponent<EnemyScanner>();
        playerBasicAttack = GetComponent<PlayerBasicAttack>();
        playerLaserAttack = GetComponent<PlayerLaserAttack>();
        playerDroneAttack = GetComponent<PlayerDroneAttack>();
        playerRandomFieldAttack = GetComponent<PlayerRandomFieldAttack>();
    }

    private void Update()
    {
        GetComponentFromDrone();
        scanner.ScanEnemy();
        AllAttack();
    }

    void GetComponentFromDrone()
    {
        bool isGet = false;
        if(isGet == false)
            droneScanner = playerDroneAttack.GetComponent<EnemyScanner>();
        isGet = true;
        droneScanner.ScanEnemy();
    }

    public void AllAttack()
    {
        //기본공격, 초월공격 활성화 코드
        if(scanner.nearCollider != null)
            playerBasicAttack.UseBasicAttack(isUpgrade, scanner.nearCollider);
        //관통공격활성화코드
        playerLaserAttack.UseLaserAttack(isLaser);
        //드론공격활성화코드
        if(droneScanner.nearCollider != null)
            playerDroneAttack.UseDroneAttack(isDrone, droneScanner.nearCollider);
        //랜덤필드공격활성화코드
        playerRandomFieldAttack.UseRandomFieldAttack(isField);
        
    }
}
