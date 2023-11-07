using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossLineRenderer : MonoBehaviour
{


    private NavMeshAgent _agent;
    private LineRenderer _line;

    private void Awake()
    {
        _agent = GetComponentInParent<NavMeshAgent>();
        _line = GetComponent<LineRenderer>();


        _line.material.color = Color.blue;
        _line.enabled = false;
    }

    public void MakePath()
    {
        _line.enabled = true;
      
        StartCoroutine(makePathCoroutine());
    }

    

    IEnumerator makePathCoroutine()
    {

        while(Vector3.Distance(this.transform.position, _agent.destination) > 0.1f)
        {
            _line.SetPosition(0, this.transform.position);

            drawPath();

            yield return null;
        }

        _line.enabled = false;
    }

    private void drawPath()
    {
        int length = _agent.path.corners.Length;
        _line.positionCount = length;

        for (int i = 0; i < length; ++i)
        {
            _line.SetPosition(i, _agent.path.corners[i]);
        }
    }
}
