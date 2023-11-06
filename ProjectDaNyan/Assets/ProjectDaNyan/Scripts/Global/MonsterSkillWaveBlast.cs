using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class MonsterSkillWaveBlast : MonoBehaviour
{
    // unity event handler 
    public UnityEvent EventHandlerWaveBlastEnd;


    // skill status data 
    [SerializeField] private MonsterBlastData _monsterBlastData;

    private SphereCollider _sphereCollider;
    private LineRenderer _lineRenderer;

    // Wave blast damage value 
    public float Damage;



    private void Awake()
    {
        // set damage 
        Damage = _monsterBlastData.Damage;

        _sphereCollider = GetComponent<SphereCollider>();
        _lineRenderer = GetComponent<LineRenderer>();


        _lineRenderer.positionCount = _monsterBlastData.PositionCount + 1;
    }

    public void StartWaveBlastAttack()
    {
        StartCoroutine(waveblast());
    }

    public void StopWaveBlastAttack()
    {
        StopCoroutine(waveblast());
    }

    private IEnumerator waveblast()
    {
        float currentRadius = 0f;

        while(currentRadius < _monsterBlastData.MaxRadius)
        {
            currentRadius += Time.deltaTime * _monsterBlastData.WaveSpeed;
            drawBlast(currentRadius);
            yield return null;
        }

        // unity event handler  
        EventHandlerWaveBlastEnd.Invoke();

        // reset collider radius
        setSphereCollider(0);
    }


    private void drawBlast(float currentRadius)
    {
        float angleBetweenPositions = 360f / _monsterBlastData.PositionCount;

        for(int i = 0; i <= _monsterBlastData.PositionCount; ++i)
        {
            float angle = i * angleBetweenPositions * Mathf.Deg2Rad;
            Vector3 direction = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle), 0f);
            Vector3 position = direction * currentRadius;

            _lineRenderer.SetPosition(i, position);
        }

        setSphereCollider(currentRadius);

        _lineRenderer.widthMultiplier = Mathf.Lerp(0f, _monsterBlastData.StartWidth, 1f - currentRadius / _monsterBlastData.MaxRadius);
    }


    
    private void setSphereCollider(float currentRadius)
    {
        // change collider position
        _sphereCollider.radius = currentRadius;
    }



    private void blastDamage(float currentRadius)
    {
        Collider[] hitObjects = Physics.OverlapSphere(transform.position, currentRadius);
        Rigidbody rb;


        for(int i = 0; i < hitObjects.Length; ++i)
        {
            if(hitObjects[i].TryGetComponent<Rigidbody>(out rb))
            {
                Vector3 direction = (hitObjects[i].transform.position - this.transform.position).normalized;
        
                rb.AddForce(direction, ForceMode.Impulse);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"I am wave touchse {other.name}");
    }
}
