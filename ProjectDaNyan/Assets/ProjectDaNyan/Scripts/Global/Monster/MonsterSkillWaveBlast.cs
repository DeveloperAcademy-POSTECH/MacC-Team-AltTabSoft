using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum SkillType
{
    WaveBlast,
    BigWave
}


public class MonsterSkillWaveBlast : MonsterAttack
{
    // unity event handler 
    public UnityEvent EventHandlerWaveBlastEnd;

    public bool isCollided = false;

    // skill status data 
    [SerializeField] private MonsterBlastData _monsterBlastData;

    public SphereCollider _sphereCollider;
    private LineRenderer _lineRenderer;
    
    private void Awake()
    {
        // set damage 
        Damage = _monsterBlastData.WaveBlastDamage;

        _sphereCollider = GetComponent<SphereCollider>();
        _lineRenderer = GetComponent<LineRenderer>();

        _lineRenderer.positionCount = _monsterBlastData.PositionCount + 1;
        
    }

    public void StartWaveBlastAttack(SkillType skill)
    {
        isCollided = false;
        switch (skill)
        {
            case SkillType.WaveBlast:
                StartCoroutine(waveblast(_monsterBlastData.WaveBlastMaxRadius,
                    _monsterBlastData.WaveBlastSpeed, _monsterBlastData.WaveBlastStartWidth));
                Damage = _monsterBlastData.WaveBlastDamage;
                break;

            case SkillType.BigWave:
                StartCoroutine(waveblast(_monsterBlastData.BigWaveMaxRadius,
                  _monsterBlastData.BigWaveSpeed, _monsterBlastData.BigWaveStartWidth));
                Damage = _monsterBlastData.BigWaveDamage;
                break;
        }
    }


    private IEnumerator waveblast(float maxRadius, float speed, float startWidth)
    {
        _lineRenderer.enabled = true;
        _sphereCollider.enabled = true;
        
        float currentRadius = 0f;

        while(currentRadius < maxRadius)
        {
            currentRadius += Time.deltaTime * speed;
            drawBlast(currentRadius, startWidth, maxRadius);
            yield return null;
        }

        // unity event handler  
        EventHandlerWaveBlastEnd.Invoke();

        _lineRenderer.enabled = false;
        
        // reset collider radius
        setSphereCollider(0);
    }


    private void drawBlast(float currentRadius, float startWidth, float maxRadius)
    {
        float angleBetweenPositions = 360f / _monsterBlastData.PositionCount;

        for(int i = 0; i <= _monsterBlastData.PositionCount; ++i)
        {
            float angle = i * angleBetweenPositions * Mathf.Deg2Rad;
            Vector3 direction = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle), 0f);
            Vector3 position = direction * currentRadius;

            _lineRenderer.SetPosition(i, position);
        }

        if (!isCollided)
        {
            setSphereCollider(currentRadius);
        }

        _lineRenderer.widthMultiplier = Mathf.Lerp(0f, startWidth, 1f - currentRadius / maxRadius);
    }


    private void setSphereCollider(float currentRadius)
    {
        // change collider position
        _sphereCollider.radius = currentRadius;
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.tag.Equals("Player"))
        {
            this.isCollided = true;
            this._sphereCollider.enabled = false;
        }
    }
}
