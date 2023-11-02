using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private GameObject _bomb;
    public enum Type {Default, Piercing, Laser, Field, Bomb};
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
        if (type != Type.Field && type != Type.Laser && type != Type.Bomb)
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
        if (other.gameObject.layer == LayerMask.NameToLayer("Monster"))
        {
            if(type != Type.Piercing && type != Type.Laser && type != Type.Bomb)
                trail.Clear();
            ObjectPoolManager.Inst.DestroyObject(this.gameObject);

            //폭탄총알이 몹에게 닿으면 몹에게 폭탄이 부착됨
            if(type == Type.Bomb && other != null && other.transform.Find("BombOnMonster") == null)
            {
                GameObject InstantBomb = ObjectPoolManager.Inst.BringObject(_bomb);
                InstantBomb.transform.parent = other.transform;
                InstantBomb.transform.position = other.transform.position + new Vector3(0, 1.9f, 0);
            }
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("FieldObject") && type != Type.Laser)
        {
            trail.Clear();
            ObjectPoolManager.Inst.DestroyObject(this.gameObject);
        }
    }
}
