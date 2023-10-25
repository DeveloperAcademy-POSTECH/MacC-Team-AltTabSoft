using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDroneAttack : MonoBehaviour
{
    [SerializeField] private AttackStatus _attackStatus;
    [SerializeField] private Transform bulletPosition;
    [SerializeField] private Transform dronePosition;
    [SerializeField] private GameObject droneBullet;
    [SerializeField] private GameObject drone;

    public int droneFireLevel = 1;

    [SerializeField] private float _droneFireRate;
    [SerializeField] private float _droneFireSpeed;

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
        targetCollider = scanner.nearCollider;
        if(scanner.nearCollider != null)
            droneChasing.DroneMoving(targetCollider.transform);
    }

    public void UseDroneAttack(bool isDrone, Collider enemyCollider)
    {
        if (isDrone)
        {
            if (isDroneAppear == false && drone != null && dronePosition != null)
            {
                CreateDrone(drone, dronePosition);
                _droneFireRate = _attackStatus.droneFireRate;
                _droneFireSpeed = _attackStatus.droneFireSpeed;
                bulletPosition = GameObject.Find("DroneBulletPosition").transform;
            }
                
            droneFireReady = _droneFireRate < droneFireDelay;
            droneFireDelay += Time.deltaTime;
            if (droneFireReady)
            {
                bulletPosition.LookAt(enemyCollider.transform);
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
        
        MakeInstantBullet(droneBullet, bulletPosition, false, _droneFireSpeed);
        yield return new WaitForSeconds(0.05f);
        MakeInstantBullet(droneBullet, bulletPosition, false, _droneFireSpeed);
        yield return new WaitForSeconds(0.05f);
        MakeInstantBullet(droneBullet, bulletPosition, false, _droneFireSpeed);
    }
}
