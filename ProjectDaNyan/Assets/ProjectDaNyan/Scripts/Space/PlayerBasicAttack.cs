using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBasicAttack : MonoBehaviour
{
    [SerializeField] private AttackStatus _attackStatus;
    [SerializeField] private float _basicFireRate;
    [SerializeField] private float _basicFireSpeed;
    [SerializeField] private float _upgradedFireRate;
    [SerializeField] private float _upgradedFireSpeed;
    
    [SerializeField] private SoundEffectController _soundEffectController;

    //public int basicFireLevel = 1;

    float _basicfireDelay; //기본 공격 딜레이
    bool _isFireReady;
    //public bool isUpgrade = true; //초월 여부

    public Transform bulletPositionGroup;
    public Transform bulletPosition; //플레이어 오브젝트에 총알이 나갈 위치를 오브젝트로 지정
    public Transform[] upgradeBulletPositions; //초월 공격에서 추가될 공격 방향 
    public GameObject basicBullet; //기본 공격 총알로 쓸 오브젝트 프리펩
    public GameObject upgradedBullet; //초월 공격 여러갈래로 퍼져나갈 총알들 프리펩

    private void OnEnable()
    {
        _basicFireRate = _attackStatus.basicFireRate;
        _basicFireSpeed = _attackStatus.basicFireSpeed;
        _upgradedFireRate = _attackStatus.upgradedFireRate;
        _upgradedFireSpeed = _attackStatus.upgradedFireSpeed;
    }

    public void UseBasicAttack(Collider enemyCollider, int basicFireLevel)
    {
        bulletPositionGroup.LookAt(enemyCollider.transform);

        _isFireReady = _basicFireRate < _basicfireDelay;
        _basicfireDelay += Time.deltaTime;
        if (_isFireReady)
        {
            _soundEffectController.playStageSoundEffect(0.2f,SoundEffectController.StageSoundTypes.Player_Attack);
            StartCoroutine(BasicAttack(basicFireLevel));
            _basicfireDelay = 0;
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

    IEnumerator BasicAttack(int basicFireLevel)
    {
        MakeInstantBullet(basicBullet, bulletPosition, false, _basicFireSpeed);
        if(basicFireLevel > 1)
        {
            MakeInstantBullet(basicBullet, upgradeBulletPositions[0], false, _basicFireSpeed);
            MakeInstantBullet(basicBullet, upgradeBulletPositions[1], false, _basicFireSpeed);
        }
        if (basicFireLevel > 2)
        {
            MakeInstantBullet(basicBullet, upgradeBulletPositions[2], false, _basicFireSpeed);
            MakeInstantBullet(basicBullet, upgradeBulletPositions[3], false, _basicFireSpeed);
        }
        if (basicFireLevel > 3)
        {
            MakeInstantBullet(basicBullet, upgradeBulletPositions[4], false, _basicFireSpeed);
            MakeInstantBullet(basicBullet, upgradeBulletPositions[5], false, _basicFireSpeed);
        }
        if (basicFireLevel > 4)
        {
            MakeInstantBullet(basicBullet, upgradeBulletPositions[6], false, _basicFireSpeed);
            MakeInstantBullet(basicBullet, upgradeBulletPositions[7], false, _basicFireSpeed);
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
