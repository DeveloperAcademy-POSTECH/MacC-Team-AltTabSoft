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
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("boxCat"))
        {
            Debug.Log("Enter");
            if (_playerState.getPsData() == PlayerState.PSData.dash)
            {
                other.gameObject.SetActive(false);
                player_now_EXP += 10;
                Debug.Log("경험치 획득! 획득한 경험치 : 10");
                Debug.Log("현재 경험치:"+player_now_EXP);
                Debug.Log("다음 레벨업까지 필요한 경험치:"+(level_Up_Require_EXP-player_now_EXP));
            }
            else
            {
                hitEnemy += 1;
                //Debug.Log("으윽! 적에게 맞았다! 10의 데미지를 입었다!");
                //player_Now_HP -= 10;
                //Debug.Log("현재 HP :"+player_Now_HP);
            }
        }
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("boxCat"))
        {
            Debug.Log(hitEnemy);
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("boxCat"))
        {
            hitEnemy -= 1;
            Debug.Log("Exit");
        }
    }
}