using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBasicAttack : MonoBehaviour
{
    public int basicFireLevel = 1;
    public float basicFireRate; //기본공격주기 
    public float upgradedFireRate; //초월공격주기 
    public float basicFireSpeed; //기본공격 투사체 속도
    public float upgradedFireSpeed; //초월공격 투사체 속도

    float basicfireDelay; //기본 공격 딜레이
    bool isFireReady;
    //public bool isUpgrade = true; //초월 여부


    public Transform bulletPosition; //플레이어 오브젝트에 총알이 나갈 위치를 오브젝트로 지정
    public Transform[] upgradeBulletPositions; //초월 공격에서 추가될 공격 방향 
    public GameObject basicBullet; //기본 공격 총알로 쓸 오브젝트 프리펩
    public GameObject upgradedBullet; //초월 공격 여러갈래로 퍼져나갈 총알들 프리펩

    EnemyScanner scanner; //가까운 적 탐지 스크립트

    public void UseBasicAttack(bool isUpgrade)
    {
        if (isUpgrade == false)
        {
            //playerBasicAttack.bulletPosition.LookAt(scanner.nearCollider.transform);
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
    }

    void MakeInstantBullet(GameObject bulletObject, Transform bulletObjectPosition, bool isGravity, float fireSpeed)
    {
        GameObject bullet = ObjectPoolManager.Inst.BringObject(bulletObject);
        bullet.transform.position = bulletObjectPosition.position;
        Rigidbody basicBulletRigid = bullet.GetComponent<Rigidbody>();
        basicBulletRigid.useGravity = isGravity;
        basicBulletRigid.velocity = bulletObjectPosition.forward * fireSpeed;
    }

    IEnumerator BasicAttack()
    {
        MakeInstantBullet(basicBullet, bulletPosition, false, basicFireSpeed);
        if(basicFireLevel > 1)
        {
            MakeInstantBullet(basicBullet, upgradeBulletPositions[2], false, basicFireSpeed);
            MakeInstantBullet(basicBullet, upgradeBulletPositions[3], false, basicFireSpeed);
        }
        if (basicFireLevel > 2)
        {
            MakeInstantBullet(basicBullet, upgradeBulletPositions[0], false, basicFireSpeed);
            MakeInstantBullet(basicBullet, upgradeBulletPositions[1], false, basicFireSpeed);
        }
        yield return null;
    }

    IEnumerator UpgradeAttack()
    {
        yield return null;
        bulletPosition.Rotate(new Vector3(0, Random.Range(-15f, 15f), 0));
        MakeInstantBullet(upgradedBullet, bulletPosition, false, upgradedFireSpeed);
        bulletPosition.localRotation = Quaternion.Euler(0, 0, 0);

    }



}
