using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "MonsterBlastData", menuName = "Scriptable_Object/MonsterBlastData")]
public class MonsterBlastData : ScriptableObject
{

    public float Damage;
    public int PositionCount;
    public float MaxRadius;
    public float WaveSpeed;
    public float StartWidth;

}
