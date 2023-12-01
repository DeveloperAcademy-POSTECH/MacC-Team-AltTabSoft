using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Restriction : MonoBehaviour
{
    private void Start()
    {
    }

    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.CompareTag("Rock"))
        {
            other.gameObject.SetActive(false);
        }
    }
}
