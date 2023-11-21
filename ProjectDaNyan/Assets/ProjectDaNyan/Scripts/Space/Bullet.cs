using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private AttackStatus _attackStatus;
    [SerializeField] private GameObject _bomb;
    [SerializeField] private GameObject _explosion;

    public enum Type
    {
        //기본공격 
        Default,
        //초월공격 
        Upgrade,
        //레이저공격 
        Laser,
        //필드공격(현재 사용 안함)
        Field,
        //폭탄공격 투사체 
        Bomb,
        //폭탄공격 폭발
        BombBlast,
        //드론공격 총알 
        Drone,
        //총알 피격 후 폭발 이펙트 
        BulletExplosion,
        //대쉬 공격 판정 총알
        DashBullet
    }


    public Type type;
    public float damage;
    //폭탄 공격 보스에게 적용시 폭발 스택
    public int bombStack;
    //총알이 가진 트레일렌더러 
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
            case Type.DashBullet:
                damage = _attackStatus.dashDamage;
                break;
            default:
                damage = 0f;
                break;
        }

        if (type != Type.BulletExplosion &&
            type != Type.DashBullet &&
            type != Type.Laser)
            trail = GetComponent<TrailRenderer>();

        if (type != Type.BombBlast &&
            type != Type.DashBullet &&
            type != Type.Laser)
            StartCoroutine(Goodbye());
    }


    //총알이 어디에도 안맞았을 경우 일정시간 후 오브젝트풀로 회수 
    IEnumerator Goodbye()
    {
        if (type != Type.BulletExplosion &&
            type != Type.BombBlast)
        {
            yield return new WaitForSeconds(1f);
            trail.Clear();
            ObjectPoolManager.Inst.DestroyObject(this.gameObject);
        }
        else if (type == Type.BulletExplosion)
        {
            yield return new WaitForSeconds(0.3f);
            ObjectPoolManager.Inst.DestroyObject(this.gameObject);
        }
        else
        {
            yield return new WaitForSeconds(4f);
            ObjectPoolManager.Inst.DestroyObject(this.gameObject);
        }
    }


    //총알이 몬스터에 닿았을 경우 폭발 이펙트를 오브젝트풀로 생성 

    IEnumerator BulletExplosion()
    {
        GameObject BulletFire = ObjectPoolManager.Inst.BringObject(_explosion);
        BulletFire.transform.position = this.gameObject.transform.position;
        yield return null;
    }

    private void OnTriggerEnter(Collider other)
    {
        //총알이 몬스터에 닿을 경우의 로직 
        if (other.gameObject.layer == LayerMask.NameToLayer("Monster") &&
            type != Type.BombBlast)
        {
            //오브젝트풀 회수하기 전에 트레일 미리 제거
            if (type != Type.Laser &&
                type != Type.BulletExplosion &&
                type != Type.DashBullet)
                trail.Clear();

            //총알이 몬스터에 닿았을 때 터지는 이펙트
            if (type == Type.Upgrade ||
               type == Type.Default ||
               type == Type.Drone) //몬스터 피격시 폭발 효과낼 총알 타입 조건 추가 필
            {
                StartCoroutine(BulletExplosion());
            }

            //총알 오브젝트풀로 회수 
            if (type != Type.BulletExplosion &&
                type != Type.DashBullet &&
                type != Type.Laser)
                ObjectPoolManager.Inst.DestroyObject(this.gameObject);


            //폭탄총알이 몹에게 닿으면 몹에게 폭탄이 부착됨
            if (type == Type.Bomb &&
                other != null &&
                (other.transform.Find("BombOnMonster") == null || other.transform.Find("BombOnMonster").gameObject.activeSelf == false))
            {
                GameObject InstantBomb = ObjectPoolManager.Inst.BringObject(_bomb);
                InstantBomb.transform.parent = other.transform;
                InstantBomb.transform.position = other.transform.position + new Vector3(0, 2.5f, 0);
            }
        }

        //총알이 맵 바닥에 닿으면 사라지는 경우
        if ((other.gameObject.layer == LayerMask.NameToLayer("Map_Object") || other.gameObject.layer == LayerMask.NameToLayer("Map_Floor")) &&
            type != Type.Laser &&
            type != Type.BombBlast &&
            type != Type.BulletExplosion &&
            type != Type.DashBullet)
        {
            trail.Clear();
            ObjectPoolManager.Inst.DestroyObject(this.gameObject);
        }
    }
}
