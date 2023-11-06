using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private AttackStatus _attackStatus;
    [SerializeField] private GameObject _bomb;

    public enum Type {Default, Upgrade, Laser, Field, Bomb, BombBlast};
    public Type type;
    public float _damage;
    TrailRenderer trail;

    private void OnEnable()
    {
        switch (type)
        {
            case Type.Default:
                _damage = _attackStatus.basicFireDamage;
                break;
            case Type.Upgrade:
                _damage = _attackStatus.upgradeFireDamage;
                break;
            case Type.Laser:
                _damage = _attackStatus.laserDamage;
                break;
            case Type.BombBlast:
                _damage = _attackStatus.bombDamage;
                break;
            default:
                _damage = 0f;
                break;
        }

        if(type != Type.Field)
            trail = GetComponent<TrailRenderer>();
        if(type != Type.BombBlast)
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
        if (other.gameObject.layer == LayerMask.NameToLayer("Monster") && type != Type.BombBlast)
        {
            if(type != Type.Laser && type != Type.Bomb)
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

        if (other.gameObject.layer == LayerMask.NameToLayer("FieldObject"))
        {
            trail.Clear();
            ObjectPoolManager.Inst.DestroyObject(this.gameObject);
        }
    }
}
