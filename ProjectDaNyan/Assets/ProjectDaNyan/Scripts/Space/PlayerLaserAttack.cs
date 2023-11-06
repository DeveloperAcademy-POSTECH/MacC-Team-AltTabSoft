using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLaserAttack : MonoBehaviour
{
    [SerializeField] private AttackStatus _attackStatus;
    [SerializeField] private GameObject[] _laserBullets; //레이저 관통 공격 오브젝트 프리펩
    [SerializeField] private GameObject _laserGroup;
    [SerializeField] private float _laserFireRate;
    

    private float _laserFireDelay; //레이저 딜레이 
    private bool _isLaserReady;

    private void OnEnable()
    {
        _laserFireRate = _attackStatus.laserFireRate;
    }

    public void UseLaserAttack(bool isLaser, int laserLevel)
    {
        if (isLaser)
        {
            _laserGroup.transform.Rotate(Vector3.up * 30 * Time.deltaTime);
            _isLaserReady = _laserFireRate < _laserFireDelay;
            _laserFireDelay += Time.deltaTime;
            if (_isLaserReady)
            {
                StartCoroutine(LaserAttack(laserLevel));
                _laserFireDelay = 0;
            }
        }
    }
    IEnumerator LaserAttack(int laserLevel)
    {
        
        _laserBullets[0].SetActive(true);
        if(laserLevel > 1)
        {
            //레이저레벨 2일 경우 활성화
            _laserBullets[1].SetActive(true);
         
        }
        if(laserLevel > 2)
        {
            //레이저레벨 3일 경우 활성화
            _laserBullets[2].SetActive(true);
           
        }
        if (laserLevel > 3)
        {
            //레이저레벨 4일 경우 활성화
            _laserBullets[3].SetActive(true);
            
        }

        yield return new WaitForSeconds(1.5f);

        for(int laserCount = 0; laserCount < _laserBullets.Length; laserCount++)
        {
            _laserBullets[laserCount].SetActive(false);
        }
    }
}
