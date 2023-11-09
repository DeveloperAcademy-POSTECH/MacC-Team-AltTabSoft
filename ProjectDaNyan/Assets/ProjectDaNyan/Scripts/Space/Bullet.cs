using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public enum Type {Default, Piercing, Laser, Field};
    public Type type;
    public int damage;
    TrailRenderer trail;


    int time = 3;

    private void OnEnable()
    {
        if(type != Type.Field)
            trail = GetComponent<TrailRenderer>();
        StartCoroutine(Goodbye());
    }


    IEnumerator Goodbye()
    {
        if (type != Type.Field)
        {
            yield return new WaitForSeconds(1f);
            trail.Clear();
            ObjectPoolManager.Inst.DestroyObject(this.gameObject);
        }
        else
        {
            yield return new WaitForSeconds(4f);
            ObjectPoolManager.Inst.DestroyObject(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Monster") && type != Type.Piercing && type != Type.Laser)
        {
            trail.Clear();
            ObjectPoolManager.Inst.DestroyObject(this.gameObject);
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("FieldObject") && type != Type.Laser)
        {
            trail.Clear();
            ObjectPoolManager.Inst.DestroyObject(this.gameObject);
        }
    }
}
