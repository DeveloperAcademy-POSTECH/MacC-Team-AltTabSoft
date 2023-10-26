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
}
