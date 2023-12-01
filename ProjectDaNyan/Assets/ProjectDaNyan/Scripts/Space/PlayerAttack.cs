using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public bool isUpgrade = true; //초월 여부
    public bool isLaser = true; //레이저 공격을 입수했는지 여부 
    public bool isDrone = false; //드론 공격 입수했는지 여부
    public bool isBomb = false; //폭탄 공격 입수했는지 여부 
    public bool isField = false; //랜덤 필드 공격 입수했는지 여부

    public int basicFireLevel = 1;
    public int laserLevel = 1;
    public int droneLevel = 1;
    public int bombLevel = 1;
    public int dashLevel = 1;
    //public int fieldAttackLevel = 1;

    private EnemyScanner _scanner; //가까운 적 탐지 스크립트
    private EnemyScanner _droneScanner; //드론의 적 탐지 스크립트
    PlayerBasicAttack playerBasicAttack;
    PlayerLaserAttack playerLaserAttack;
    PlayerDroneAttack playerDroneAttack;
    PlayerDroneAttack playerDrone2Attack;
    PlayerBombAttack playerBombAttack;
    PlayerRandomFieldAttack playerRandomFieldAttack;
    [SerializeField] private GameObject _drone;
    [SerializeField] private GameObject _drone2;
    [SerializeField] private AttackStatus _attackStatus;
    

    private void Awake()
    {

        //캐릭터의 적 스캔
        _scanner = GetComponent<EnemyScanner>();
        _scanner.scanRange = _attackStatus.playerScanRange;

        //각 공격들 컴포넌트 가져오기
        playerBasicAttack = GetComponent<PlayerBasicAttack>();
        playerLaserAttack = GetComponent<PlayerLaserAttack>();
        playerBombAttack = GetComponent<PlayerBombAttack>();
        playerRandomFieldAttack = GetComponent<PlayerRandomFieldAttack>();
        playerDroneAttack = GameObject.Find("Drone").GetComponent<PlayerDroneAttack>();
        playerDrone2Attack = GameObject.Find("Drone2").GetComponent<PlayerDroneAttack>();
        //필드에 생성된 드론 찾기
        _drone = GameObject.Find("Drone");
        _drone.SetActive(false);
        _drone2 = GameObject.Find("Drone2");
        _drone2.SetActive(false);
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

            if(droneLevel > 5)
            {
                if (_drone2.activeSelf == false)
                {
                    _drone2.SetActive(true);
                }

                if (Vector3.Distance(_drone2.transform.position, this.transform.position) > 30)
                {
                    _drone2.transform.position = this.transform.position + new Vector3(0, 1.9f, 0);
                }
            }
   
        }

        _scanner.ScanEnemy();

        AllAttack();
        
    }

    //드론 컴포넌트 Get -> 수정 예정
    //void GetDroneScanner()
    //{
    //    bool isGet = false;
    //    if (isGet == false)
    //    {
    //        playerDroneAttack = GameObject.Find("Drone").GetComponent<PlayerDroneAttack>();
    //    }
    //    isGet = true;
    //}

    //public IEnumerator LaserAttack()
    //{
    //    //관통공격활성화코드
    //    isLaser = true;
    //    playerLaserAttack.UseLaserAttack(isLaser, laserLevel);
    //    yield return new WaitForSeconds(10f);
    //    isLaser = false;
    //}

    public void AllAttack()
    {
        //기본공격, 초월공격 활성화 코드
        if(_scanner.nearCollider != null && _scanner.nearCollider.gameObject.activeSelf == true)
            playerBasicAttack.UseBasicAttack(_scanner.nearCollider, basicFireLevel);

        

        //드론공격활성화코드
        if (isDrone)
        {
            //if (_droneScanner.nearCollider != null)
            playerDroneAttack.UseDroneAttack(isDrone, droneLevel);
            if(_drone2.activeSelf == true)
            {
                playerDrone2Attack.UseDroneAttack(isDrone, 4);
            }
        }
        //폭탄공격활성화코드
        if (_scanner.nearCollider != null)
            playerBombAttack.UseBombAttack(isBomb, _scanner.nearCollider, bombLevel);

        //랜덤필드공격활성화코드
        playerRandomFieldAttack.UseRandomFieldAttack(isField);
        
    }
}
