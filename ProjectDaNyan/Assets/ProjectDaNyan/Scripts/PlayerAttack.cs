using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public enum Type { Basic, Upgraded , Laser};//타입은 따로 bullet 스크립트를 만들어서 빼낼 예정
    public Type type;
    public float basicFireRate; //기본공격주기 
    public float upgradedFireRate; //초월공격주기 
    public float laserFireRate; //관통공격주기 
    public float basicFireSpeed; //기본공격 투사체 속도
    public float upgradedFireSpeed; //초월공격 투사체 속도
   
    
    public Transform bulletPosition; //플레이어 오브젝트에 총알이 나갈 위치를 오브젝트로 지정 
    public GameObject basicBullet; //기본 공격 총알로 쓸 오브젝트 프리펩
    public GameObject upgradedBullet; //초월 공격 여러갈래로 퍼져나갈 총알들 프리펩
    public GameObject laserBullet; //레이저 관통 공격 오브젝트 프리

    float basicfireDelay;
    bool isFireReady;

    private void Update()
    {
        Use();
    }

    public void Use()
    {
        isFireReady = basicFireRate < basicfireDelay;
        basicfireDelay += Time.deltaTime;
        if (isFireReady)
        {
            StartCoroutine("BasicAttack");
            basicfireDelay = 0;
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
        yield return null;
        //초월 공격 로
    }

    IEnumerator LaserAttack()
    {
        yield return null;
        //Laser 관통 공격 로직 
    }
    
}
