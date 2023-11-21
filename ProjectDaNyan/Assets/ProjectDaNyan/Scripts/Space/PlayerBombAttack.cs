using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBombAttack : MonoBehaviour
{
    [SerializeField] private AttackStatus _attackStatus;
    [SerializeField] private Transform _bulletPosition;
    [SerializeField] private GameObject _bombBullet;//폭탄이 발사될 때 나오는 이펙
    [SerializeField] private GameObject _bomb;//적의 머리 위에 떠있는 폭탄
    [SerializeField] private float _bombFireRate;
    [SerializeField] private float _bombFireSpeed;
    private PlayerAttack _playerAttack;
    private int _bombLevel;
    [SerializeField] private SoundEffectController _soundEffectController;

    float _bombFireDelay;
    bool _isFireReady;

    private void OnEnable()
    {
        _playerAttack = GameObject.Find("PlayerAttackPosition").GetComponent<PlayerAttack>();
        _bombFireRate = _attackStatus.bombFireRate;
        _bombFireSpeed = _attackStatus.bombFireSpeed;
    }

    private void Update()
    {
        _bombLevel = _playerAttack.bombLevel;
        if (_bombLevel > 4)
            _bombFireRate = _attackStatus.bombFireRate * 0.5f;
    }

    public void UseBombAttack(bool isBomb,Collider enemyCollider, int bombFireLevel)
    {
        _bulletPosition.LookAt(enemyCollider.transform);
        if (isBomb)
        {
            _isFireReady = _bombFireRate < _bombFireDelay;
            _bombFireDelay += Time.deltaTime;
            if (_isFireReady)
            {
                _soundEffectController.playStageSoundEffect(0.25f,SoundEffectController.StageSoundTypes.Player_Bombplant_Attack);
                StartCoroutine(BombAttack(bombFireLevel));
                _bombFireDelay = 0;
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

    IEnumerator BombAttack(int bombFireLevel)
    {
        yield return null;
        MakeInstantBullet(_bombBullet, _bulletPosition, false, _bombFireSpeed);
    }


}
