using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EneymyScanner : MonoBehaviour
{
    
    public Collider[] colliders;
    public Collider nearCollider;
    public LayerMask layer;
    public int scanRange;

    void Update()
    {
        ScanEnemy();
    }

    public void ScanEnemy()
    {
        float maxRange = Mathf.Infinity;
        foreach (Collider objectCollider in colliders)
        {
            float distance = Vector3.Distance(transform.position, objectCollider.transform.position);
            if (distance < maxRange)
            {
                nearCollider = objectCollider;
                maxRange = distance;
            }
        }
    }
}
