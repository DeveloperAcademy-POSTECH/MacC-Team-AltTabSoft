using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    [SerializeField] private PlayerState _playerState;
    [SerializeField] private int player_Max_HP = 100;
    [SerializeField] private int hp_Heal_Counter_x50 = 50;
    [SerializeField] private int hp_Heal_Amount = 1;
    [SerializeField] private int level_Up_Require_EXP = 100;

    private int player_HitCount = 0;
    
    public int Level_Up_Require_EXP
    {
        get { return level_Up_Require_EXP; }
    }

    public int Player_now_EXP
    {
        get { return player_now_EXP; }
        set { player_now_EXP = value; }
    }

    public int Player_Max_HP
    {
        get { return player_Max_HP; }
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
    
    // Start is called before the first frame update
    void Start()
    {
        player_Now_HP = player_Max_HP;
    }

    private void FixedUpdate()
    {
        TimeHealHP();
        PlayerHit();
    }

    void PlayerHit()
    {
        player_HitCount += 1;
        if (player_HitCount == 5)
        {
            player_Now_HP -= hitEnemy * 1;
            Debug.Log(hitEnemy+"명의 적에게 맞고 있다!");
            Debug.Log(hitEnemy+"의 데미지를 입었다! 현재 남은 HP:"+player_Now_HP);

            player_HitCount = 0;
        }
    }
    void TimeHealHP()
    {
        if (player_Now_HP != player_Max_HP)
        {
            heal_Cooltime += 1;
            if (heal_Cooltime == hp_Heal_Counter_x50)
            {
                heal_Cooltime = 0;
                player_Now_HP += hp_Heal_Amount;

                if (player_Now_HP > player_Max_HP)
                {
                    player_Now_HP = player_Max_HP;
                }
            }
        }
        else
        {
            heal_Cooltime = 0;
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }

    public void daamge(int damge)
    {
        player_Now_HP -= daamge();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            hitEnemy += 1;
            Debug.Log("으윽! 적에게 맞았다! 10의 데미지를 입었다!");
            player_Now_HP -= 10;
            Debug.Log("현재 HP :"+player_Now_HP);
        }
        else if (other.CompareTag("boxCat"))
        {
            //other.gameObject.SetActive(false);
            player_now_EXP += 10;
            Debug.Log("고양이를 구했다! 획득한 경험치 : 10");
            if ((level_Up_Require_EXP - player_now_EXP) < 0)
            {
                player_Level += 1;
                player_now_EXP -= level_Up_Require_EXP;
                Debug.Log("Level UP! 현재레벨:" + player_Level);
                Debug.Log("현재 경험치:"+player_now_EXP);
                Debug.Log("다음 레벨업까지 필요한 경험치:" + (level_Up_Require_EXP - player_now_EXP));
            }
            else
            {
                Debug.Log("현재 경험치:"+player_now_EXP);
                Debug.Log("다음 레벨업까지 필요한 경험치:"+(level_Up_Require_EXP-player_now_EXP));
            }
        }
    }
    
    private void OnTriggerStay(Collider other)
    {
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            hitEnemy -= 1;
        }
    }
}