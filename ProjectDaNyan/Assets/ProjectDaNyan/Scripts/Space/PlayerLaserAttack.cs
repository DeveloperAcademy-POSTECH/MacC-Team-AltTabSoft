using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLaserAttack : MonoBehaviour
{
    public float laserFireRate; //관통공격주기 
    public GameObject laserBullet; //레이저 관통 공격 오브젝트 프리펩

    IEnumerator LaserAttack()
    {
        laserBullet.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        laserBullet.SetActive(false);
    }
}
