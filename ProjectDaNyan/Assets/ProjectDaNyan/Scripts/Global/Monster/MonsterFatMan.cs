using System;
using System.Collections;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.Serialization;

public class MonsterFatMan : MonsterAttack
{
    [SerializeField] private GameObject _explosionPrefab;
    [SerializeField] private GameObject _fatManBody;
    [SerializeField] private MonsterSkillWaveBlast _monsterSkillWaveBlast;
    
    [SerializeField] private float _explosionDamageDecrease;

    private Rigidbody _rigidbody;
    private GameObject _blast;
    private GameObject _afterExplosion;
    private CapsuleCollider _collider;
    private GameObject _target;

    [SerializeField] private bool isBombed = false;
   
    // bomb target prefab 
    [SerializeField] private GameObject _fatManTargetCirclePrefab;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<CapsuleCollider>();
        _monsterSkillWaveBlast.Damage = this.Damage * _explosionDamageDecrease;
        
        _monsterSkillWaveBlast.EventHandlerWaveBlastEnd.AddListener(waveBlastEnd);
    }
    
    private void OnEnable()
    {
        isBombed = false;
        
        _fatManBody.SetActive(true);
        _collider.enabled = true;
        
        StartCoroutine(MakeTargetMark());
    }


    IEnumerator MakeTargetMark()
    {
        yield return new WaitForSeconds(0.1f);
        
        _target = ObjectPoolManager.Inst.BringObject(_fatManTargetCirclePrefab);
        _target.transform.position = new Vector3(this.transform.position.x, 1, this.transform.position.z);
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (!isBombed)
        {
            isBombed = true;

            _rigidbody.velocity = Vector3.zero;
            ObjectPoolManager.Inst.DestroyObject(_target);
            
            _collider.enabled = false;
            _fatManBody.SetActive(false);
            
            FatManExplosion();
            
            _monsterSkillWaveBlast.StartWaveBlastAttack(WaveType.WaveBlast);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        _monsterSkillWaveBlast.BlastSphereCollider.enabled = false;
        _monsterSkillWaveBlast.isCollided = true;
    }
    
    
    private void FatManExplosion()
    {
        // generate exlposion particle effect 
        _afterExplosion = ObjectPoolManager.Inst.BringObject(_explosionPrefab);
        _afterExplosion.transform.position = this.gameObject.transform.position;
    }
    private void waveBlastEnd()
    {
        ObjectPoolManager.Inst.DestroyObject(_afterExplosion);
        ObjectPoolManager.Inst.DestroyObject(this.gameObject);
    }

}
