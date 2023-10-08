using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public float basicFireRate; //기본공격주기 
    public float upgradedFireRate; //초월공격주기 
    public float laserFireRate; //관통공격주기 
    public float basicFireSpeed; //기본공격 투사체 속도
    public float upgradedFireSpeed; //초월공격 투사체 속도
   
    
    public Transform bulletPosition; //플레이어 오브젝트에 총알이 나갈 위치를 오브젝트로 지정
    public Transform[] upgradeBulletPositions; //초월 공격에서 추가될 공격 방향 
    public GameObject basicBullet; //기본 공격 총알로 쓸 오브젝트 프리펩
    public GameObject upgradedBullet; //초월 공격 여러갈래로 퍼져나갈 총알들 프리펩
    public GameObject laserBullet; //레이저 관통 공격 오브젝트 프리

    float basicfireDelay;
    bool isFireReady;
    bool isUpgrade = true;

    private void Update()
    {
        Use();
    }

    public void Use()
    {
        if (isUpgrade == false)
        {
            isFireReady = basicFireRate < basicfireDelay;
            basicfireDelay += Time.deltaTime;
            if (isFireReady)
            {
                StartCoroutine("BasicAttack");
                basicfireDelay = 0;
            }
        }
        else
        {
            isFireReady = upgradedFireRate < basicfireDelay;
            basicfireDelay += Time.deltaTime;
            if (isFireReady)
            {
                StartCoroutine("UpgradeAttack");
                basicfireDelay = 0;
            }
        }
        
            
        //StartCoroutine("LaserAttack");
    }

    IEnumerator BasicAttack()
    {
        //bulletPosition.LookAt(); //EnemyScanner로 스캔한 가까운 적 조
        GameObject instantBasicBullet = Instantiate(basicBullet, bulletPosition.position, Quaternion.identity);
        Rigidbody basicBulletRigid = instantBasicBullet.GetComponent<Rigidbody>();
        basicBulletRigid.useGravity = false;
        basicBulletRigid.velocity = bulletPosition.forward * basicFireSpeed;
        //basicBulletRigid.velocity = new Vector3(1,0,2).normalized * basicFireSpeed;
        yield return null;
        //초월 공격 로

    }

    IEnumerator UpgradeAttack()
    {
        GameObject instantBasicBullet = Instantiate(upgradedBullet, bulletPosition.position, Quaternion.identity);
        Rigidbody upgradedBulletRigid = instantBasicBullet.GetComponent<Rigidbody>();
        upgradedBulletRigid.useGravity = false;
        upgradedBulletRigid.velocity = bulletPosition.forward * upgradedFireSpeed;

        GameObject instantBasicBullet1 = Instantiate(upgradedBullet, upgradeBulletPositions[0].position, Quaternion.identity);
        Rigidbody upgradedBulletRigid1 = instantBasicBullet1.GetComponent<Rigidbody>();
        upgradedBulletRigid1.useGravity = false;
        upgradedBulletRigid1.velocity = upgradeBulletPositions[0].forward * upgradedFireSpeed;

        GameObject instantBasicBullet2 = Instantiate(upgradedBullet, upgradeBulletPositions[1].position, Quaternion.identity);
        Rigidbody upgradedBulletRigid2 = instantBasicBullet2.GetComponent<Rigidbody>();
        upgradedBulletRigid2.useGravity = false;
        upgradedBulletRigid2.velocity = upgradeBulletPositions[1].forward * upgradedFireSpeed;

        GameObject instantBasicBullet3 = Instantiate(upgradedBullet, upgradeBulletPositions[2].position, Quaternion.identity);
        Rigidbody upgradedBulletRigid3 = instantBasicBullet3.GetComponent<Rigidbody>();
        upgradedBulletRigid3.useGravity = false;
        upgradedBulletRigid3.velocity = upgradeBulletPositions[2].forward * upgradedFireSpeed;

        GameObject instantBasicBullet4 = Instantiate(upgradedBullet, upgradeBulletPositions[3].position, Quaternion.identity);
        Rigidbody upgradedBulletRigid4 = instantBasicBullet4.GetComponent<Rigidbody>();
        upgradedBulletRigid4.useGravity = false;
        upgradedBulletRigid4.velocity = upgradeBulletPositions[3].forward * upgradedFireSpeed;

        yield return null;

    }

    IEnumerator LaserAttack()
    {
        yield return null;
        //Laser 관통 공격 로직 
    }
    
}
