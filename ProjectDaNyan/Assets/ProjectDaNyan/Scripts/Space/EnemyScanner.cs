using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScanner : MonoBehaviour
{
    
    public Collider[] colliders; //scanRange 내의 에너미의 collider 모음
    public Collider nearCollider; //가장 가까운 에너미의 collider
    public LayerMask layer; //에너미 인식 기준이 되는 레이
    public int scanRange; //인식 범위

    void Update()
    {
        ScanEnemy();
    }

    private void ScanEnemy()
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
