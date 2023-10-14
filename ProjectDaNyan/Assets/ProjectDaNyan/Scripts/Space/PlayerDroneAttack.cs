using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDroneAttack : MonoBehaviour
{
    public Transform bulletPosition;
    public GameObject droneBullet;
    public GameObject drone;

    public int droneFireLevel = 1;
    public float droneSpeed;
    public float droneFireRate;
    public float droneFireSpeed;
    public bool isDroneAppear;

    float droneFireDelay;
    bool droneFireReady;

    //EnemyScanner scanner;

    //private void OnEnable()
    //{
    //    scanner = GetComponent<EnemyScanner>();
    //}

    public void UseDroneAttack(bool isDrone)
    {
        if (isDrone)
        {
            drone.SetActive(true);
            droneFireReady = droneFireRate < droneFireDelay;
            droneFireDelay += Time.deltaTime;
            if (droneFireReady)
            {
                StartCoroutine("DroneFire");
                droneFireDelay = 0;
            }
        }
    }

    void MakeInstantBullet(GameObject bulletObject, Transform bulletObjectPosition, bool isGravity, float fireSpeed)
    {
        GameObject bullet = ObejectPoolManager.Inst.BringObject(bulletObject);
        bullet.transform.position = bulletObjectPosition.position;
        Rigidbody basicBulletRigid = bullet.GetComponent<Rigidbody>();
        basicBulletRigid.useGravity = isGravity;
        basicBulletRigid.velocity = bulletObjectPosition.forward * fireSpeed;
    }

    IEnumerator DroneFire()
    {
        yield return null;
        MakeInstantBullet(droneBullet, bulletPosition, false, droneFireSpeed);
    }
}
