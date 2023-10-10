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
        AllAttack();
    }

    public void AllAttack()
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

    void MakeInstantBullet(GameObject bulletObject, Transform bulletObjectPosition, bool isGravity, float fireSpeed)
    {
        GameObject instantBasicBullet = Instantiate(bulletObject, bulletObjectPosition.position, Quaternion.identity);
        Rigidbody basicBulletRigid = instantBasicBullet.GetComponent<Rigidbody>();
        basicBulletRigid.useGravity = isGravity;
        basicBulletRigid.velocity = bulletObjectPosition.forward * fireSpeed;
    }

    IEnumerator BasicAttack()
    {
        MakeInstantBullet(basicBullet,bulletPosition,false,basicFireSpeed);
        yield return null;
    }

    IEnumerator UpgradeAttack()
    {
        MakeInstantBullet(upgradedBullet, bulletPosition, false, upgradedFireSpeed);
        MakeInstantBullet(upgradedBullet, upgradeBulletPositions[0], false, upgradedFireSpeed);
        MakeInstantBullet(upgradedBullet, upgradeBulletPositions[1], false, upgradedFireSpeed);
        MakeInstantBullet(upgradedBullet, upgradeBulletPositions[2], false, upgradedFireSpeed);
        MakeInstantBullet(upgradedBullet, upgradeBulletPositions[3], false, upgradedFireSpeed);
        yield return null;

    }

    IEnumerator LaserAttack()
    {
        yield return null;
        //Laser 관통 공격 로직 
    }
    
}
