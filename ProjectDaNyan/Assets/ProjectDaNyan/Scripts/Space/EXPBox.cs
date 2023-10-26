using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EXPBox : MonoBehaviour
{

    [SerializeField] private float popForce = 5f;


    Rigidbody _rigidbody = null;
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();

    }
    
    private void OnEnable()
    {
        _rigidbody.velocity = Vector3.up * popForce;
    }

    private void OnTriggerExit(Collider other)
    {
        // if possible, check by layer 
        if (other.gameObject.layer == 10)
        {
            Debug.Log("layer 10");
            ObjectPoolManager.Inst.DestroyObject(this.gameObject);
        }

        if(other.name == "MapFloor")
        {
            ObjectPoolManager.Inst.DestroyObject(this.gameObject);
        }
    }
}
