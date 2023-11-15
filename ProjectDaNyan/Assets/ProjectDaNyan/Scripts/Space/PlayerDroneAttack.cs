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
    private int _enemyNumber;
    private int _randomNumber;

    DroneChaseMonster droneChasing;
    EnemyScanner scanner;
    public Collider targetCollider;

    private void OnEnable()
    {
        scanner = GetComponent<EnemyScanner>();
        droneChasing = GetComponent<DroneChaseMonster>();
        scanner.scanRange = _attackStatus.droneScanRange;
        //dronePosition = GameObject.Find("DronePosition").transform;

    }

    private void Update()
    {
        
        droneChasing.StartCoroutine("ReturnToPlayer");
        //targetCollider = scanner.nearCollider;
        if(scanner.nearCollider != null)
            droneChasing.DroneMoving(targetCollider.transform);
    }

    public void UseDroneAttack(bool isDrone, int droneLevel)
    {
        
        scanner.ScanEnemy();

        if(scanner.colliders.Length > 0)
        {
            _enemyNumber = scanner.colliders.Length;
            _randomNumber = Random.Range(0, _enemyNumber);
            targetCollider = scanner.colliders[_randomNumber];
        }
        
        
        if (isDrone && targetCollider != null && targetCollider.gameObject.activeSelf == true)
        {
            _droneFireRate = _attackStatus.droneFireRate;
            _droneFireSpeed = _attackStatus.droneFireSpeed;
            //_bulletPosition = GameObject.Find("DroneBulletPosition").transform;
            _bulletPosition = gameObject.transform.GetChild(0);
            _droneFireReady = _droneFireRate < _droneFireDelay;
            _droneFireDelay += Time.deltaTime;
            _drone.transform.RotateAround(targetCollider.transform.position, Vector3.up, 30 * Time.deltaTime);

            if (_droneFireReady)
            {
                if (droneLevel > 4)
                    droneLevel = 4;
                    
                _bulletPosition.LookAt(targetCollider.transform);
                StartCoroutine(DroneFire(droneLevel));
                _droneFireDelay = 0;
            }


        }
    }

    void MakeInstantBullet(GameObject bulletObject, Transform bulletObjectPosition, bool isGravity, float fireSpeed)
    {
        GameObject bullet = ObjectPoolManager.Inst.BringObject(bulletObject);
        bullet.transform.position = bulletObjectPosition.position;
        bullet.transform.LookAt(targetCollider.transform);
        Rigidbody basicBulletRigid = bullet.GetComponent<Rigidbody>();
        basicBulletRigid.useGravity = isGravity;
        basicBulletRigid.velocity = bulletObjectPosition.forward * fireSpeed;
    }

    IEnumerator DroneFire(int droneLevel)
    {
        for (int i = 0; i < droneLevel; i++)
        {
            scanner.ScanEnemy();
            MakeInstantBullet(_droneBullet, _bulletPosition, false, _droneFireSpeed);
            yield return new WaitForSeconds(0.08f);
        }
    }
}
