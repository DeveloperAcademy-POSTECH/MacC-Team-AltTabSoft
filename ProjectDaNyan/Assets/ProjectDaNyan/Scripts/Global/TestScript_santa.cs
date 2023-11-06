using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript_santa : MonoBehaviour
{

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"I am {this.name} collides with {collision.gameObject.tag}");

    }


}
