using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneChaseMonster : MonoBehaviour
{
    public GameObject player;
    public Transform thisPosition;
    UnityEngine.AI.NavMeshAgent agent;

    void Awake()
    {
        player = GameObject.Find("PlayerAttackPosition");
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    public void DroneMoving(Transform target)
    {
        float distance = Vector3.Distance(player.transform.position, thisPosition.position);
        StartCoroutine(FollowEnemy(target));

        if (distance > 10 || target == null)
        {
            StartCoroutine(ReturnToPlayer());
        }
        else
        {
            StartCoroutine(FollowEnemy(target));
        }
    }

    IEnumerator FollowEnemy(Transform target)
    {
        yield return null;
        if (target != null)
            agent.SetDestination(target.position);
        else
            agent.SetDestination(player.transform.position);
    }

    IEnumerator ReturnToPlayer()
    {
        yield return null;
        agent.SetDestination(player.transform.position);
    }
}
