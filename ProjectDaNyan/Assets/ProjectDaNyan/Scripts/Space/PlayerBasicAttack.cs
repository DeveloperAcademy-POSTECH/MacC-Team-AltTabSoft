using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBasicAttack : MonoBehaviour
{
    [SerializeField] private AttackStatus _attackStatus;

    public int basicFireLevel = 1;
    //public float _basicFireRate; //기본공격주기 
    //public float _upgradedFireRate; //초월공격주기 
    //public float _basicFireSpeed; //기본공격 투사체 속도
    //public float _upgradedFireSpeed; //초월공격 투사체 속도

    [SerializeField] private float _basicFireRate;
    [SerializeField] private float _basicFireSpeed;
    [SerializeField] private float _upgradedFireRate;
    [SerializeField] private float _upgradedFireSpeed;

    float _basicfireDelay; //기본 공격 딜레이
    bool _isFireReady;
    //public bool isUpgrade = true; //초월 여부

    public Transform bulletPositionGroup;
    public Transform bulletPosition; //플레이어 오브젝트에 총알이 나갈 위치를 오브젝트로 지정
    public Transform[] upgradeBulletPositions; //초월 공격에서 추가될 공격 방향 
    public GameObject basicBullet; //기본 공격 총알로 쓸 오브젝트 프리펩
    public GameObject upgradedBullet; //초월 공격 여러갈래로 퍼져나갈 총알들 프리펩

    EnemyScanner scanner; //가까운 적 탐지 스크립트

    private void OnEnable()
    {
        _basicFireRate = _attackStatus.basicFireRate;
        _basicFireSpeed = _attackStatus.basicFireSpeed;
        _upgradedFireRate = _attackStatus.upgradedFireRate;
        _upgradedFireSpeed = _attackStatus.upgradedFireSpeed;
    }

    public void UseBasicAttack(bool isUpgrade, Collider enemyCollider)
    {
        bulletPositionGroup.LookAt(enemyCollider.transform);
        if (isUpgrade == false)
        {
            
            _isFireReady = _basicFireRate < _basicfireDelay;
            _basicfireDelay += Time.deltaTime;
            if (_isFireReady)
            {
                StartCoroutine("BasicAttack");
                _basicfireDelay = 0;
            }
        }
        else
        {
            _isFireReady = _upgradedFireRate < _basicfireDelay;
            _basicfireDelay += Time.deltaTime;
            if (_isFireReady)
            {
                StartCoroutine("UpgradeAttack");
                _basicfireDelay = 0;
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
        MakeInstantBullet(basicBullet, bulletPosition, false, _basicFireSpeed);
        if(basicFireLevel > 1)
        {
            MakeInstantBullet(basicBullet, upgradeBulletPositions[2], false, _basicFireSpeed);
            MakeInstantBullet(basicBullet, upgradeBulletPositions[3], false, _basicFireSpeed);
        }
        if (basicFireLevel > 2)
        {
            MakeInstantBullet(basicBullet, upgradeBulletPositions[0], false, _basicFireSpeed);
            MakeInstantBullet(basicBullet, upgradeBulletPositions[1], false, _basicFireSpeed);
        }
        yield return null;
    }

    IEnumerator UpgradeAttack()
    {
        yield return null;
        bulletPosition.Rotate(new Vector3(0, Random.Range(-15f, 15f), 0));
        MakeInstantBullet(upgradedBullet, bulletPosition, false, _upgradedFireSpeed);
        bulletPosition.localRotation = Quaternion.Euler(0, 0, 0);

    }



}
