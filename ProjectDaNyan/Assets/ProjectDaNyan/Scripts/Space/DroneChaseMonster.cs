using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneChaseMonster : MonoBehaviour
{
    public Transform player;
    public Transform target;
    EnemyScanner enemyScanner;
    UnityEngine.AI.NavMeshAgent agent;

    bool isReturn = false;
    
    void Awake()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        enemyScanner = GetComponent<EnemyScanner>();
    }


    private void Update()
    {
        enemyScanner.ScanEnemy();
        if(enemyScanner.nearCollider != null)
        {
            target = enemyScanner.nearCollider.transform;
            if (isReturn!)
            {
                agent.SetDestination(target.position);
            }
            else
            {
                returnToPlayer();
            }
        }
       
        
    }

    void returnToPlayer()
    {
        float distance = Vector3.Distance(player.position, target.position);
        if (distance > 50)
        {
            agent.SetDestination(player.position);
        }
        else if(distance < 10)
        {
            isReturn = false;
        }

    }
}
