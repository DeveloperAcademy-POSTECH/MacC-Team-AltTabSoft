using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterRangeAttack : MonoBehaviour
{
    public Transform[] bulletPositions; //플레이어 오브젝트에 총알이 나갈 위치를 오브젝트로 지정
    public GameObject basicBullet; //기본 공격 총알로 쓸 오브젝트 프리펩
    private float _bulletSpeed; //기본공격 투사체 속도
    
    public void BulletAttack()
    {
        StartCoroutine("FireBullet");
    }
    
    IEnumerator FireBullet()
    {
        foreach (Transform tr in bulletPositions)
        {
            MakeInstantBullet(basicBullet, tr, false, _bulletSpeed);

        }
        yield return null;
    }

    private void MakeInstantBullet(GameObject bulletObject, Transform bulletObjectPosition, bool isGravity, float fireSpeed)
    {
        GameObject bullet = ObjectPoolManager.Inst.BringObject(bulletObject);
        bullet.transform.position = bulletObjectPosition.position;
        Rigidbody basicBulletRigid = bullet.GetComponent<Rigidbody>();
        basicBulletRigid.useGravity = isGravity;
        basicBulletRigid.velocity = bulletObjectPosition.forward * fireSpeed;
    }
}
