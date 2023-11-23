using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLaserAttack : MonoBehaviour
{
    [SerializeField] private GameObject _playerAttackPosition;
    [SerializeField] private AttackStatus _attackStatus;
    [SerializeField] private GameObject[] _laserBullets; //레이저 관통 공격 오브젝트 프리펩
    [SerializeField] private GameObject _laserGroup;
    [SerializeField] private float _laserFireRate;
    [SerializeField] private SoundEffectController _soundEffectController;
    

    private float _laserFireDelay; //레이저 딜레이 
    private bool _isLaserReady;

    private void OnEnable()
    {
        _laserFireRate = _attackStatus.laserFireRate;
        _laserGroup.transform.SetParent(null);
        for (int laserCount = 0; laserCount < _laserBullets.Length; laserCount++)
        {
            _laserBullets[laserCount].SetActive(false);
        }


    }

    private void Update()
    {
        _laserGroup.transform.position = _playerAttackPosition.transform.position + new Vector3(0, -1.3f, 0); 
        _laserGroup.transform.Rotate(Vector3.up * 30 * Time.deltaTime);


    }

    public void UseLaserAttack(bool isLaser ,int laserLevel)
    {
        if (isLaser)
        {
            _soundEffectController.playStageSoundEffect(0.5f,SoundEffectController.StageSoundTypes.Player_Hidden_Laser);
            _laserGroup.transform.Rotate(Vector3.up * 30 * Time.deltaTime);
            _laserGroup.transform.position = _playerAttackPosition.transform.position;
            StartCoroutine(LaserAttack(laserLevel));
            
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

        yield return new WaitForSeconds(5f);

        for(int laserCount = 0; laserCount < _laserBullets.Length; laserCount++)
        {
            _laserBullets[laserCount].SetActive(false);
        }

    }
}
