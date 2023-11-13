using UnityEngine;
using UnityEngine.Pool;

public enum MonsterType
{
    Normal,
    Elite,
    Boss
}

public class Monster : MonoBehaviour
{
    public IObjectPool<GameObject> myPool { get; set; }

    public MonsterType myType;
}

