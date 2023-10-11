using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{

    float basicfireDelay; //기본 공격 딜레이 
    float laserFireDelay; //레이저 딜레이 
    bool isFireReady;
    bool isLaserReady;
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
        if (isUpgrade == false)
        {
            playerBasicAttack.bulletPosition.LookAt(scanner.nearCollider.transform);
            isFireReady = playerBasicAttack.basicFireRate < basicfireDelay;
            basicfireDelay += Time.deltaTime;
            if (isFireReady)
            {
                playerBasicAttack.StartCoroutine("BasicAttack");
                basicfireDelay = 0;
            }
        }
        else
        {
            isFireReady = playerBasicAttack.upgradedFireRate < basicfireDelay;
            basicfireDelay += Time.deltaTime;
            if (isFireReady)
            {
                playerBasicAttack.StartCoroutine("UpgradeAttack");
                basicfireDelay = 0;
            }
        }
        //관통공격활성화코드 
        if (isLaser)
        {
            isLaserReady = playerLaserAttack.laserFireRate < laserFireDelay;
            laserFireDelay += Time.deltaTime;
            if (isLaserReady)
            {
                playerLaserAttack.StartCoroutine("LaserAttack");
                laserFireDelay = 0;
            }
        }
    }
}
