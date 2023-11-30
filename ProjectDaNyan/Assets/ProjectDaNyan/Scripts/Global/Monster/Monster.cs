using UnityEngine;
using UnityEngine.Pool;
using System.Collections;
using System.Collections.Generic;

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
    
    [Header("Monster Status")]
    // boss monstser status
    public float monsterHP;
    [SerializeField] protected Renderer[] _renderers;
    
    //Bomb explosion objects
    [Header("Bomb Explosion")]
    [SerializeField] protected GameObject _boom;
    [SerializeField] protected GameObject _boomCollider;
    [SerializeField] protected GameObject _bomb; //Bomb on the Boss Monster
    [SerializeField] protected PlayerAttack _playerAttack;
    public int _bombLevel;

    // material 
    // private Queue<Color> _originColorList = new Queue<Color>();

    private Queue<Color> _originalColors = new Queue<Color>();
    
    private void Awake()
    {
        //PlayerAttack Bomb Skill Information
        _playerAttack = GameObject.Find("PlayerAttackPosition").GetComponent<PlayerAttack>();
    }
    
    
    private void Update()
    {
        _bombLevel = _playerAttack.bombLevel;
    }
    
    
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("PlayerAttack"))
        {
            // get bullet damage 
            if (other.gameObject.TryGetComponent(out Bullet bullet))
            {
                // apply player attack damage 
                applyDamage(bullet.damage);
                
                // change color 
                StartCoroutine(monsterHit());
                
                if (this.gameObject.transform.Find("BombOnMonster") != null &&
                    this.gameObject.transform.Find("BombOnMonster").gameObject.activeSelf == true)
                {
                    _bomb = this.gameObject.transform.Find("BombOnMonster").gameObject;
                    bullet.bombStack += 1;
                    StartCoroutine(bombExplosion(bullet, _bomb, _bombLevel, 0.5f));
                }

                if (bullet.type == Bullet.Type.Bomb && this.gameObject.transform.Find("BombOnMonster") != null &&
                    this.gameObject.transform.Find("BombOnMonster").gameObject.activeSelf == false)
                {
                    _bomb.SetActive(true);
                }
            }
            // if bullet doesn't have damage 
            else
            {
                applyDamage(1);
            }
        }
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (other.tag.Equals("PlayerAttack"))
        {
            // change color 
            StartCoroutine(monsterHit());
            
            // get bullet damage 
            if (other.gameObject.TryGetComponent(out Bullet bullet))
            {
                // apply player attack damage 
                applyDamage(bullet.damage);
            }
            // if bullet doesn't have damage 
            else
            {
                applyDamage(1);
            }
        }
    }
    

    IEnumerator monsterHit()
    {
        foreach (Renderer ren in _renderers)
        {
            _originalColors.Enqueue(ren.material.color);
            ren.material.color = Color.red;
        }
        
        yield return new WaitForSeconds(0.25f);
        
        foreach (Renderer ren in _renderers)
        {
            ren.material.color = _originalColors.Dequeue();
        }
    }
    
    
    IEnumerator bombExplosion(Bullet bullet, GameObject bomb, int bombLevel, float boomSize)
    {
        if (bullet.bombStack > 20) //default 20
        {
            if (bombLevel > 4)
                bombLevel = 4;
            bullet.bombStack = 0;
            //몬스터 위에 있는 폭탄 비활성
            bomb.gameObject.transform.SetParent(null);
            ObjectPoolManager.Inst.DestroyObject(bomb);
            //bomb.SetActive(false);
            //폭발 파티클 이펙트
            GameObject boomEffect = ObjectPoolManager.Inst.BringObject(_boom);
            boomEffect.transform.localScale = new Vector3(boomSize + (0.25f * bombLevel) , boomSize + (0.25f * bombLevel), boomSize + (0.25f * bombLevel));
            boomEffect.transform.position = this.gameObject.transform.position + new Vector3(0, 1f, 0);

            //터지는 순간 위에서 안보이는 Collider가 떨어지면서 Trigger 발동
            GameObject boomCollider = ObjectPoolManager.Inst.BringObject(_boomCollider);
            boomCollider.transform.localScale = new Vector3(boomSize + (0.25f * bombLevel), boomSize + (0.25f * bombLevel), boomSize + (0.25f * bombLevel));
            boomCollider.transform.position = this.gameObject.transform.position + new Vector3(0, 10, 0);
            Rigidbody boomColliderRigid = boomCollider.GetComponent<Rigidbody>();
            boomColliderRigid.velocity = boomCollider.transform.up * -100f;

            yield return new WaitForSeconds(1f);
            ObjectPoolManager.Inst.DestroyObject(boomEffect);
            ObjectPoolManager.Inst.DestroyObject(boomCollider);
        }
    }

    // apply damage 
    protected virtual void applyDamage(float damage)
    {
        monsterHP -= damage;
    }
}

