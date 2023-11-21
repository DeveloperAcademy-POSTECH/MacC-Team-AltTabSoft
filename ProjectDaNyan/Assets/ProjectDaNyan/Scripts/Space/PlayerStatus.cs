using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    [SerializeField] private PlayerData _playerData;
    [SerializeField] private PlayerState _playerState;
    [SerializeField] private Renderer playerRenderer;

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

    private int player_Level = 1;

    private int player_now_EXP = 0;
    private int player_Now_HP = 100;

    private int heal_Cooltime = 0;

    private int hitEnemy = 0;
    private int player_collected_box_cat = 0;
    
    private int dashCharged;
    public int DashCharged
    {
        get { return dashCharged; }
        set { dashCharged = value; }
    }
    private int dashRechargeTimer = 0;
    
    public int Player_collected_box_cat
    {
        get { return player_collected_box_cat; }
    }


    // Start is called before the first frame update
    void Start()
    {
        player_Now_HP = _playerData.player_Max_HP;
        dashCharged = _playerData.maxDashSavings;
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
            player_Now_HP -= 10;
            Debug.Log("10의 데미지를 입었다.");
            
            StartCoroutine(PlayerHitEffect());
        }
        else if (other.collider.gameObject.CompareTag("MonsterAttack"))
        {
            player_Now_HP -= (int)other.gameObject.GetComponent<MonsterAttack>().Damage;
            //other.gameObject.SetActive(false);
            Debug.Log($"총에 맞았다! 총 데미지를 입었다. {other.gameObject.GetComponent<MonsterAttack>().Damage}");
            StartCoroutine(PlayerHitEffect());
        }
    }
    private void OnCollisionExit(Collision other)
    {
        /*
        if (other.gameObject.CompareTag("Monster"))
        {
            hitEnemy -= 1;
        }
        */
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("boxCat"))
        {
            other.gameObject.SetActive(false);
            player_now_EXP += 10;
            Debug.Log("경험치 획득! 획득한 경험치 : 10");
            Debug.Log("현재 경험치:"+player_now_EXP);
            Debug.Log("다음 레벨업까지 필요한 경험치:"+(_playerData.level_Up_Require_EXP-player_now_EXP));
            
            ItemController.Inst.DropItem();
            
            // TODO: 드랍 아이템별 카운트 구현
            player_collected_box_cat += 1;
        }
    }
    
    private void OnTriggerStay(Collider other)
    {
    }
    
    private void OnTriggerExit(Collider other)
    {
        
    }
}