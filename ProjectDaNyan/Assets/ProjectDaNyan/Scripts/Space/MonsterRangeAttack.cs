using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterRangeAttack : MonoBehaviour
{
    public float basicFireRate; //기본공격주기 
    public float basicFireSpeed; //기본공격 투사체 속도

    public Transform bulletPosition; //플레이어 오브젝트에 총알이 나갈 위치를 오브젝트로 지정
    public GameObject basicBullet; //기본 공격 총알로 쓸 오브젝트 프리펩

    float basicfireDelay; //기본 공격 딜레이
    bool isFireReady;

    private void Update()
    {
        UseBasicAttack();
    }

    public void UseBasicAttack()
    {
        isFireReady = basicFireRate < basicfireDelay;
        basicfireDelay += Time.deltaTime;
        if (isFireReady)
        {
            StartCoroutine("BasicAttack");
            basicfireDelay = 0;
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
        yield return null;
    }
}
