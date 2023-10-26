using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLaserAttack : MonoBehaviour
{
    [SerializeField] private AttackStatus _attackStatus;
    [SerializeField] private GameObject laserBullet; //레이저 관통 공격 오브젝트 프리펩
    [SerializeField] private float _laserFireRate;

    private float _laserFireDelay; //레이저 딜레이 
    private bool _isLaserReady;

    private void OnEnable()
    {
        _laserFireRate = _attackStatus.laserFireRate;
    }

    public void UseLaserAttack(bool isLaser)
    {
        if (isLaser)
        {
            _isLaserReady = _laserFireRate < _laserFireDelay;
            _laserFireDelay += Time.deltaTime;
            if (_isLaserReady)
            {
                StartCoroutine("LaserAttack");
                _laserFireDelay = 0;
            }
        }
    }
    IEnumerator LaserAttack()
    {
        laserBullet.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        laserBullet.SetActive(false);
    }
}
