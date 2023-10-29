using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EXPBox : MonoBehaviour
{
    [SerializeField] private float popForce = 5f;

    // depends on monster's exp value 
    public float exp;

    public Vector3 parentsVelocity;

    Rigidbody _rigidbody = null;
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
    
    private void OnEnable()
    {
        _rigidbody.AddForce(parentsVelocity * popForce, ForceMode.Impulse);
        _rigidbody.AddForce(Vector3.up * popForce, ForceMode.Impulse);
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.name == "MapFloor")
        {
            ObjectPoolManager.Inst.DestroyObject(this.gameObject);
        }
    }
}
