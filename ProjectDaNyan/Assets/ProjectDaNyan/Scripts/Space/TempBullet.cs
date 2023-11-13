using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempBullet : MonsterAttack
{
    TrailRenderer _tR;

    [SerializeField] private GameObject _explosion;
    
    // disappear bullet after 3 seconds  
    private float _destroyTime = 3f;
    private float _isShootingTime = 0f;
    

    // Start is called before the first frame update
     private void Awake()
    {
        _tR = GetComponent<TrailRenderer>();   
    }

    // Update is called once per frame
    private void Update()
    {
        _isShootingTime += Time.deltaTime;

        if(_isShootingTime >= _destroyTime)
        {
            _tR.Clear();
            _isShootingTime = 0f;
            ObjectPoolManager.Inst.DestroyObject(this.gameObject);
            return;
        }
    }

    private void OnDisable()
    {
        _tR.Clear();
    }


    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {
            // apply damage 
            Debug.Log("Collision Player!");
            GameObject afterExplosion = ObjectPoolManager.Inst.BringObject(_explosion);
            afterExplosion.transform.position = this.gameObject.transform.position;
        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            ObjectPoolManager.Inst.DestroyObject(this.gameObject);
        }
    }
}
