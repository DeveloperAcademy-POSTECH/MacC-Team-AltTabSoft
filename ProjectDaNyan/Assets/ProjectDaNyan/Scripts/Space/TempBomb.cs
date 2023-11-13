using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempBomb : MonsterAttack
{
    [SerializeField] private GameObject _explosion;

    private Quaternion originPos;
    private Rigidbody _rigidbody;
    
    // private void Awake()
    // {
    //     originPos = this.transform.rotation;
    //     _rigidbody = GetComponent<Rigidbody>();
    // }
    //
    // private void OnEnable()
    // {
    //     this.transform.rotation = originPos;
    //     _rigidbody.velocity = Vector3.zero;
    // }


    private void OnCollisionEnter(Collision other)
    {
        GameObject afterExplosion = ObjectPoolManager.Inst.BringObject(_explosion);
        afterExplosion.transform.position = this.gameObject.transform.position;
        
        ObjectPoolManager.Inst.DestroyObject(this.gameObject);
    }
}
