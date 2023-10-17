using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDroneAttack : MonoBehaviour
{
    public Transform bulletPosition;
    public Transform dronePosition;
    public GameObject droneBullet;
    public GameObject drone;

    public int droneFireLevel = 1;
    public float droneSpeed;
    public float droneFireRate;
    public float droneFireSpeed;
    bool isDroneAppear = false;

    float droneFireDelay;
    bool droneFireReady;

    DroneChaseMonster droneChasing;
    EnemyScanner scanner;
    public Collider targetCollider;

    private void OnEnable()
    {
        scanner = GetComponent<EnemyScanner>();
        droneChasing = GetComponent<DroneChaseMonster>();
        dronePosition = GameObject.Find("DronePosition").transform;

    }

    private void Update()
    {
        droneChasing.StartCoroutine("ReturnToPlayer");
        scanner.ScanEnemy();
        targetCollider = scanner.nearCollider;
        if(scanner.nearCollider != null)
            droneChasing.DroneMoving(targetCollider.transform);
    }

    public void UseDroneAttack(bool isDrone)
    {
        if (isDrone)
        {
            if (isDroneAppear == false && drone != null && dronePosition != null)
            {
                CreateDrone(drone, dronePosition);
                bulletPosition = GameObject.Find("DroneBulletPosition").transform;
            }
                
            droneFireReady = droneFireRate < droneFireDelay;
            droneFireDelay += Time.deltaTime;
            if (droneFireReady)
            {
                StartCoroutine("DroneFire");
                droneFireDelay = 0;
            }
        }
    }

    void CreateDrone(GameObject droneObject, Transform droneObjectPosition)
    {
        
        GameObject createdDrone = ObjectPoolManager.Inst.BringObject(droneObject);
        createdDrone.transform.position = droneObjectPosition.position;
        isDroneAppear = true;

    }

    void MakeInstantBullet(GameObject bulletObject, Transform bulletObjectPosition, bool isGravity, float fireSpeed)
    {
        GameObject bullet = ObjectPoolManager.Inst.BringObject(bulletObject);
        bullet.transform.position = bulletObjectPosition.position;
        Rigidbody basicBulletRigid = bullet.GetComponent<Rigidbody>();
        basicBulletRigid.useGravity = isGravity;
        basicBulletRigid.velocity = bulletObjectPosition.forward * fireSpeed;
    }

    IEnumerator DroneFire()
    {
        
        MakeInstantBullet(droneBullet, bulletPosition, false, droneFireSpeed);
        yield return new WaitForSeconds(0.05f);
        MakeInstantBullet(droneBullet, bulletPosition, false, droneFireSpeed);
        yield return new WaitForSeconds(0.05f);
        MakeInstantBullet(droneBullet, bulletPosition, false, droneFireSpeed);
    }
}
