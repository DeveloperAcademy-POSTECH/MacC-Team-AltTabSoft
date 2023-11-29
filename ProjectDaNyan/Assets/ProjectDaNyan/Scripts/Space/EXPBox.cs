
using UnityEngine;
using UnityEngine.Pool;

public class EXPBox : MonoBehaviour
{
    public IObjectPool<GameObject> myPool { get; set; }

    // depends on monster's exp value 
    public float exp;
}
