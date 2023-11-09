using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private AttackStatus _attackStatus;
    [SerializeField] private GameObject _bomb;
    [SerializeField] private GameObject _explosion;

    public enum Type {Default, Upgrade, Laser, Field, Bomb, BombBlast, Drone};
    public Type type;
    public float damage;
    public int bombStack;
    TrailRenderer trail;
    
    private void OnEnable()
    {
        switch (type)
        {
            case Type.Default:
                damage = _attackStatus.basicFireDamage;
                break;
            case Type.Upgrade:
                damage = _attackStatus.upgradeFireDamage;
                break;
            case Type.Laser:
                damage = _attackStatus.laserDamage;
                break;
            case Type.BombBlast:
                damage = _attackStatus.bombDamage;
                break;
            case Type.Drone:
                damage = _attackStatus.droneDamage;
                break;
            default:
                damage = 0f;
                break;
        }

        if(type != Type.Field)
            trail = GetComponent<TrailRenderer>();
        if(type != Type.BombBlast)
            StartCoroutine(Goodbye());
    }



    IEnumerator Goodbye()
    {
        if (type != Type.Field && type != Type.Laser && type != Type.BombBlast)
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

    IEnumerator BulletExplosion()
    {
        GameObject afterExplosion = ObjectPoolManager.Inst.BringObject(_explosion);
        afterExplosion.transform.position = this.gameObject.transform.position;
        yield return new WaitForSeconds(0.3f);
        ObjectPoolManager.Inst.DestroyObject(afterExplosion);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Monster") && type != Type.BombBlast)
        {
            if(type != Type.Laser)
                trail.Clear();
            if(type == Type.Upgrade && type == Type.Default) //몬스터 피격시 폭발 효과낼 총알 타입 조건 추가 필
            {
                StartCoroutine(BulletExplosion());
                //GameObject afterExplosion = ObjectPoolManager.Inst.BringObject(_explosion);
                //afterExplosion.transform.position = this.gameObject.transform.position;
                //ObjectPoolManager.Inst.DestroyObject(afterExplosion);
            }
            
            ObjectPoolManager.Inst.DestroyObject(this.gameObject);
            

            //폭탄총알이 몹에게 닿으면 몹에게 폭탄이 부착됨
            if(type == Type.Bomb && other != null && (other.transform.Find("BombOnMonster") == null || other.transform.Find("BombOnMonster").gameObject.activeSelf == false))
            {
                GameObject InstantBomb = ObjectPoolManager.Inst.BringObject(_bomb);         
                InstantBomb.transform.parent = other.transform;
                InstantBomb.transform.position = other.transform.position + new Vector3(0, 2.5f, 0);
            }
        }

        if ((other.gameObject.layer == LayerMask.NameToLayer("Map_Object") || other.gameObject.layer == LayerMask.NameToLayer("Map_Floor")) && type != Type.Laser && type != Type.BombBlast)
        {
            trail.Clear();
            ObjectPoolManager.Inst.DestroyObject(this.gameObject);
        }
    }
}
