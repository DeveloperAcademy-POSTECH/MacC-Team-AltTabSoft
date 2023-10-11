using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBasicAttack : MonoBehaviour
{
    public float basicFireRate; //기본공격주기 
    public float upgradedFireRate; //초월공격주기 
    public float basicFireSpeed; //기본공격 투사체 속도
    public float upgradedFireSpeed; //초월공격 투사체 속도


    public Transform bulletPosition; //플레이어 오브젝트에 총알이 나갈 위치를 오브젝트로 지정
    public Transform[] upgradeBulletPositions; //초월 공격에서 추가될 공격 방향 
    public GameObject basicBullet; //기본 공격 총알로 쓸 오브젝트 프리펩
    public GameObject upgradedBullet; //초월 공격 여러갈래로 퍼져나갈 총알들 프리펩

    EnemyScanner scanner; //가까운 적 탐지 스크립트

    void MakeInstantBullet(GameObject bulletObject, Transform bulletObjectPosition, bool isGravity, float fireSpeed)
    {
        GameObject instantBasicBullet = Instantiate(bulletObject, bulletObjectPosition.position, Quaternion.identity);
        Rigidbody basicBulletRigid = instantBasicBullet.GetComponent<Rigidbody>();
        basicBulletRigid.useGravity = isGravity;
        basicBulletRigid.velocity = bulletObjectPosition.forward * fireSpeed;
    }

    IEnumerator BasicAttack()
    {
        MakeInstantBullet(basicBullet, bulletPosition, false, basicFireSpeed);
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

        //bulletPosition.Rotate(new Vector3(0, Random.Range(-15f, 15f), 0));
        //upgradeBulletPositions[2].Rotate(new Vector3(0, Random.Range(-15f, 15f), 0));
        //upgradeBulletPositions[3].Rotate(new Vector3(0, Random.Range(-15f, 15f), 0));
        //MakeInstantBullet(upgradedBullet, bulletPosition, false, upgradedFireSpeed);
        //MakeInstantBullet(upgradedBullet, upgradeBulletPositions[2], false, upgradedFireSpeed);
        //MakeInstantBullet(upgradedBullet, upgradeBulletPositions[3], false, upgradedFireSpeed);
        //bulletPosition.localRotation = Quaternion.Euler(0, 0, 0);
        //upgradeBulletPositions[2].localRotation = Quaternion.Euler(0, 0, 0);
        //upgradeBulletPositions[3].localRotation = Quaternion.Euler(0, 0, 0);

    }
}
