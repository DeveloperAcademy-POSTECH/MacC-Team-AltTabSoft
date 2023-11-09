using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDroneAttack : MonoBehaviour
{
    [SerializeField] private AttackStatus _attackStatus;
    [SerializeField] private Transform _bulletPosition;
    //[SerializeField] private Transform dronePosition;
    [SerializeField] private GameObject _droneBullet;
    [SerializeField] private GameObject _drone;
    [SerializeField] private float _droneFireRate;
    [SerializeField] private float _droneFireSpeed;

    public int droneFireLevel = 1;
    private float _droneFireDelay;
    private bool _droneFireReady;

    DroneChaseMonster droneChasing;
    EnemyScanner scanner;
    public Collider targetCollider;

    private void OnEnable()
    {
        scanner = GetComponent<EnemyScanner>();
        droneChasing = GetComponent<DroneChaseMonster>();
        //dronePosition = GameObject.Find("DronePosition").transform;

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
            _droneFireRate = _attackStatus.droneFireRate;
            _droneFireSpeed = _attackStatus.droneFireSpeed;
            _bulletPosition = GameObject.Find("DroneBulletPosition").transform;
            _droneFireReady = _droneFireRate < _droneFireDelay;
            _droneFireDelay += Time.deltaTime;
            _drone.transform.RotateAround(enemyCollider.transform.position,Vector3.up, 30 * Time.deltaTime) ;
            if (_droneFireReady)
            {
                _bulletPosition.LookAt(enemyCollider.transform);
                StartCoroutine("DroneFire");
                _droneFireDelay = 0;
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

    IEnumerator DroneFire()
    {
        MakeInstantBullet(_droneBullet, _bulletPosition, false, _droneFireSpeed);
        yield return new WaitForSeconds(0.05f);
        MakeInstantBullet(_droneBullet, _bulletPosition, false, _droneFireSpeed);
        yield return new WaitForSeconds(0.05f);
        MakeInstantBullet(_droneBullet, _bulletPosition, false, _droneFireSpeed);
    }
}
