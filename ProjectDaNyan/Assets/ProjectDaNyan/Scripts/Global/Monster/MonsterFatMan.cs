using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterFatMan : MonsterAttack
{
    [SerializeField] private GameObject _explosion;
    [SerializeField] private GameObject _fatManCollider;
    [SerializeField] private GameObject _fatManBody;
    [SerializeField] private float _explosionDamageDecrease;
    private Quaternion originPos;
    
    private void OnEnable()
    {
        _fatManBody.SetActive(true);
    }
    
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.gameObject.tag.Equals("Player") || 
            (other.gameObject.layer == LayerMask.NameToLayer("Map_Object") || 
             other.gameObject.layer == LayerMask.NameToLayer("Map_Floor")))
        {
            _fatManBody.SetActive(false);
            StartCoroutine(fatManExplosion());
        }
    }


    IEnumerator fatManExplosion()
    {
        // generate exlposion particle effect 
        GameObject afterExplosion = ObjectPoolManager.Inst.BringObject(_explosion);
        afterExplosion.transform.position = this.gameObject.transform.position;

        // // drop collider 
        // GameObject fatManCollider = ObjectPoolManager.Inst.BringObject(_fatManCollider);
        // fatManCollider.transform.position = this.gameObject.transform.position + new Vector3(0, 5, 0);
        // Rigidbody fatManrigidbody = _fatManCollider.GetComponent<Rigidbody>();
        // fatManrigidbody.velocity = Vector3.up * -100f;
        //
        // // set explosion damage 
        // fatManCollider.GetComponent<MonsterAttack>().Damage = base.Damage * _explosionDamageDecrease;
        
        
        yield return new WaitForSeconds(0.2f);
        
        ObjectPoolManager.Inst.DestroyObject(afterExplosion);
        
        ObjectPoolManager.Inst.DestroyObject(this.gameObject);
    }
}
