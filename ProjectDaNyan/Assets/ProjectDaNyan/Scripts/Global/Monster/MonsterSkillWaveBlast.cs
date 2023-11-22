using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public enum WaveType
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

    public SphereCollider BlastSphereCollider;
    private LineRenderer _blastLineRenderer;
    
    private void Awake()
    {
        // set damage 
        Damage = _monsterBlastData.WaveBlastDamage;

        BlastSphereCollider = GetComponent<SphereCollider>();
        _blastLineRenderer = GetComponent<LineRenderer>();

        _blastLineRenderer.positionCount = _monsterBlastData.PositionCount + 1;
        
    }

    public void StartWaveBlastAttack(WaveType skill)
    {
        isCollided = false;
        
        switch (skill)
        {
            case WaveType.WaveBlast:
                StartCoroutine(waveblast(_monsterBlastData.WaveBlastMaxRadius,
                    _monsterBlastData.WaveBlastSpeed, _monsterBlastData.WaveBlastStartWidth));
                Damage = _monsterBlastData.WaveBlastDamage;
                break;

            case WaveType.BigWave:
                StartCoroutine(waveblast(_monsterBlastData.BigWaveMaxRadius,
                  _monsterBlastData.BigWaveSpeed, _monsterBlastData.BigWaveStartWidth));
                Damage = _monsterBlastData.BigWaveDamage;
                break;
        }
    }


    public void ResetWave()
    {
        _blastLineRenderer.positionCount = 0;
        BlastSphereCollider.radius = 0;
    }
    
    private IEnumerator waveblast(float maxRadius, float speed, float startWidth)
    {
        
        _blastLineRenderer.enabled = true;
        BlastSphereCollider.enabled = true;
        
        float currentRadius = 0f;
        

        while(currentRadius < maxRadius)
        {
            currentRadius += Time.deltaTime * speed;
            drawBlast(currentRadius, startWidth, maxRadius);
            yield return null;
        }

        // unity event handler  
        EventHandlerWaveBlastEnd.Invoke();
        
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

            _blastLineRenderer.SetPosition(i, position);
        }

        if (!isCollided)
        {
            setSphereCollider(currentRadius);
        }

        _blastLineRenderer.widthMultiplier = Mathf.Lerp(0f, startWidth, 1f - currentRadius / maxRadius);
    }


    private void setSphereCollider(float currentRadius)
    {
        // change collider position
        BlastSphereCollider.radius = currentRadius;
    }
}
