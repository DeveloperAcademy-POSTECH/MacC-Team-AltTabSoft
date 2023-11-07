using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "MonsterBlastData", menuName = "Scriptable_Object/MonsterBlastData")]
public class MonsterBlastData : ScriptableObject
{
    // Damage of wave blast  
    public float WaveBlastDamage;

    // Damage of big wave
    public float BigWaveDamage;

    // line renderer position counts 
    public int PositionCount;

    // size of wave blast 
    public float WaveBlastMaxRadius;

    // size of big wave
    public float BigWaveMaxRadius;

    // speed of wave blast 
    public float WaveBlastSpeed;

    // speed of big wave
    public float BigWaveSpeed;

    // width of wave blast 
    public float WaveBlastStartWidth;

    // width of big wave
    public float BigWaveStartWidth;
}
