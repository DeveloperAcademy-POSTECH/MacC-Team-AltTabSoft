
using UnityEngine;
using UnityEngine.Pool;

public class EXPBox : MonoBehaviour
{
    public IObjectPool<GameObject> myPool { get; set; }
    
    [SerializeField] private float popForce = 20f;

    // depends on monster's exp value 
    public float exp;

    public Vector3 parentsVelocity;

    Rigidbody _rigidbody = null;
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        _rigidbody.constraints = RigidbodyConstraints.None;
        _rigidbody.AddForce(parentsVelocity * popForce, ForceMode.Impulse);
        _rigidbody.AddForce(Vector3.up * popForce, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Map_Floor"))
        {
            _rigidbody.velocity = Vector3.zero;
        }
    }
    
    
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Map_Floor"))
        {
            _rigidbody.constraints = RigidbodyConstraints.FreezeAll;
            _rigidbody.velocity = Vector3.zero;
        }
    }
}
