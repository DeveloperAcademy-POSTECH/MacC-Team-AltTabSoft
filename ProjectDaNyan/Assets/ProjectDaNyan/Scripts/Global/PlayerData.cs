using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable_Object/PlayerData")]

public class PlayerData: ScriptableObject
{
    // ======= PlayerController에서 사용될 데이터
    [SerializeField] private int playerSpeed = 10;
    [SerializeField] private int dashSpeed = 10;
    [SerializeField] private int dashLimitTic1SecondsTo50 = 10; //50틱 = 1초, 대시가 지속될 시간 설정
    [SerializeField] private int onTheRockQuitTimer = 25; //50틱 = 1초, 이 시간 안에 조이스틱 미조작 시 바위에서 강제사출
    
    // ======= PlayerStatus에서 사용될 데이터
    [SerializeField] private int player_Max_HP = 100;
    [SerializeField] private int hp_Heal_Counter_x50 = 50;
    [SerializeField] private int hp_Heal_Amount = 1;
    [SerializeField] private int level_Up_Require_EXP = 100;
}
