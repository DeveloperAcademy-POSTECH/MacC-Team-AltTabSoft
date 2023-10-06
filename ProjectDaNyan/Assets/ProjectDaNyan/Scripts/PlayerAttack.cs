using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public enum Type { Basic, Upgraded , Laser};
    public Type type;
    public float basicFireRate;
    public float UpgradedFireRate;
    public float laserFireRate;

    public Transform bulletPosition;
    public GameObject basicBullet;
    public GameObject[] upgradedBullet;
    public GameObject laserBullet;

    IEnumerator BasicAttack()
    {
        yield return null;
        //basic과 upgraded 공격 로직
    }

    IEnumerator LaserAttack()
    {
        yield return null;
        //Laser 관통 공격 로직
    }
    
    void Update()
    {
        
    }
}
