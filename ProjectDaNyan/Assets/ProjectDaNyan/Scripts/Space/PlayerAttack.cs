using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public bool isUpgrade = true; //초월 여부
    public bool isLaser = true; //레이저 공격을 입수했는지 여부 
    public bool isDrone = false; //드론 공격 입수했는지 여부
    public bool isField = false; //랜덤 필드 공격 입수했는지 여부

    public int basicFireLevel = 1;
    public int laserLevel = 1;
    //public int droneLevel = 1;
    //public int fieldAttackLevel = 1;

    private EnemyScanner _scanner; //가까운 적 탐지 스크립트
    private EnemyScanner _droneScanner; //드론의 적 탐지 스크립트
    PlayerBasicAttack playerBasicAttack;
    PlayerLaserAttack playerLaserAttack;
    PlayerDroneAttack playerDroneAttack;
    PlayerRandomFieldAttack playerRandomFieldAttack;
    [SerializeField] private GameObject _drone;
    [SerializeField] private AttackStatus _attackStatus;
    

    private void Awake()
    {
        _drone = GameObject.Find("Drone");
        _drone.SetActive(false);
        _scanner = GetComponent<EnemyScanner>();
        _scanner.scanRange = _attackStatus.playerScanRange;
        playerBasicAttack = GetComponent<PlayerBasicAttack>();
        playerLaserAttack = GetComponent<PlayerLaserAttack>();
        playerRandomFieldAttack = GetComponent<PlayerRandomFieldAttack>();
    }

    private void Update()
    {
        if (isDrone)
        {
            if (_drone.activeSelf == false)
            {
                _drone.SetActive(true);
            }

            if(Vector3.Distance(_drone.transform.position, this.transform.position) > 30)
            {
                _drone.transform.position = this.transform.position + new Vector3(0, 1.9f, 0);
            }

            GetDroneScanner();
        }
        _scanner.ScanEnemy();
        AllAttack();
        
    }

    void GetDroneScanner()
    {
        bool isGet = false;
        if (isGet == false)
        {
            playerDroneAttack = GameObject.Find("Drone").GetComponent<PlayerDroneAttack>();
            _droneScanner = GameObject.Find("Drone").GetComponent<EnemyScanner>();
            _droneScanner.scanRange = _attackStatus.droneScanRange;
        }
        isGet = true;
        _droneScanner.ScanEnemy();
    }

    public void AllAttack()
    {
        //기본공격, 초월공격 활성화 코드
        if(_scanner.nearCollider != null)
            playerBasicAttack.UseBasicAttack(isUpgrade, _scanner.nearCollider, basicFireLevel);

        //관통공격활성화코드
        playerLaserAttack.UseLaserAttack(isLaser, laserLevel);

        //드론공격활성화코드
        if (isDrone)
        {
            if (_droneScanner.nearCollider != null)
                playerDroneAttack.UseDroneAttack(isDrone, _droneScanner.nearCollider);
        }
        
        //랜덤필드공격활성화코드
        playerRandomFieldAttack.UseRandomFieldAttack(isField);
        
    }
}
