using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    [SerializeField] private PlayerData _playerData;
    [SerializeField] private PlayerState _playerState;
    [SerializeField] private Renderer playerRenderer;
    [SerializeField] private SoundEffectController soundEffectController;

    private int player_HitCount = 0;

    public int Level_Up_Require_EXP
    {
        get { return _playerData.level_Up_Require_EXP; }
    }

    public int Player_now_EXP
    {
        get { return player_now_EXP; }
        set { player_now_EXP = value; }
    }

    public int Player_Max_HP
    {
        get { return _playerData.player_Max_HP; }
    }

    public int Player_Now_HP
    {
        get { return player_Now_HP; }
    }

    public int player_Level = 1;

    private int player_now_EXP = 0;
    public int player_Now_HP = 100;

    private int heal_Cooltime = 0;

    private int hitEnemy = 0;
    private int player_collected_box_cat = 0;
    private int player_collected_gold = 0;
    
    private int dashCharged;
    public int DashCharged
    {
        get { return dashCharged; }
        set { dashCharged = value; }
    }
    private int dashRechargeTimer = 0;
    private int dashSpeed = 0;

    public int DashSpeed
    {
        get { return dashSpeed; }
        set { dashSpeed = value; }
    }

    public int Player_collected_box_cat
    {
        get { return player_collected_box_cat; }
        set { player_collected_box_cat = value; }
    }
    
    public int Player_collected_gold
    {
        get { return player_collected_gold; }
        set { player_collected_gold = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        player_Now_HP = _playerData.player_Max_HP;
        dashCharged = _playerData.maxDashSavings;
        // _playerData.dashSpeed = 10; //게임 리셋마다 스크립터블에서 지정한 값으로 초기화 필요 
        dashSpeed = _playerData.dashSpeed;
    }

    private void FixedUpdate()
    {
        TimeHealHP();
        PlayerHit();
        PlayerDead();
        PlayerDashRecharge();
    }
    
    void PlayerDashRecharge()
    {
        if (_playerState.getPsData() != PlayerState.PSData.onTheRock)
        {
            if (dashCharged < _playerData.maxDashSavings)
            {
                dashRechargeTimer += 1;
                if (dashRechargeTimer >= _playerData.dashRechargeTic)
                {
                    dashRechargeTimer = 0;
                    dashCharged += 1;
                }
            }
        }
    }
    
    void PlayerHit()
    {
        player_HitCount += 1;
        if (player_HitCount == 5)
        {
            player_Now_HP -= hitEnemy * 1;
            player_HitCount = 0;
        }
    }

    void PlayerDead()
    {
        if (player_Now_HP <= 0)
        {
            GameManager.Inst.PlayerDead();
        }
    }

    void TimeHealHP()
    {
        if (player_Now_HP != _playerData.player_Max_HP)
        {
            heal_Cooltime += 1;
            if (heal_Cooltime == _playerData.hp_Heal_Counter_x50)
            {
                heal_Cooltime = 0;
                player_Now_HP += _playerData.hp_Heal_Amount;

                if (player_Now_HP > _playerData.player_Max_HP)
                {
                    player_Now_HP = _playerData.player_Max_HP;
                }
            }
        }
        else
        {
            heal_Cooltime = 0;
        }
    }

    IEnumerator PlayerHitEffect()
    { 
        playerRenderer.material.color = Color.red;
        yield return new WaitForSeconds(_playerData.hitEffectTime);
        playerRenderer.material.color = Color.white;
        yield break;
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.gameObject.CompareTag("Monster"))
        {
            Debug.Log($"{other.gameObject.name} : 몬스터와 충돌! 10의 데미지를 입었다.");

            ApplyDamage(10);
            
            StartCoroutine(PlayerHitEffect());
        }
        else if (other.collider.gameObject.CompareTag("MonsterAttack"))
        {
            ApplyDamage((int)other.gameObject.GetComponent<MonsterAttack>().Damage);
            //other.gameObject.SetActive(false);
            Debug.Log($"{other.gameObject.name} : 몬스터 공격! {other.gameObject.GetComponent<MonsterAttack>().Damage}");
            StartCoroutine(PlayerHitEffect());
        }
    }
    
    private void ApplyDamage(int damage)
    {
        player_Now_HP -= damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MonsterAttack"))
        {
            ApplyDamage((int)other.gameObject.GetComponent<MonsterAttack>().Damage);
            //other.gameObject.SetActive(false);
            Debug.Log($"{other.gameObject.name} : 몬스터 공격! {other.gameObject.GetComponent<MonsterAttack>().Damage}");
            StartCoroutine(PlayerHitEffect());
        }
        
        if (other.CompareTag("boxCat") || other.CompareTag("EXPBox"))
        {
            other.gameObject.SetActive(false);
            player_now_EXP += 10;
            // 플레이어 레벨업 로직을 UI에서 마무리하고 싶다면 주석 처리할 것
            // if (_playerData.level_Up_Require_EXP - player_now_EXP <= 0)
            // {
            //     player_Level += 1;
            //     player_now_EXP -= _playerData.level_Up_Require_EXP;
            // }

            ItemController.Inst.DropItem();
            
            soundEffectController.playStageSoundEffect(0.5f,SoundEffectController.StageSoundTypes.Boxcat_Gold);
        }

    }
}