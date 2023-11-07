using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[CreateAssetMenu(fileName = "GameManagerData", menuName = "Scriptable_Object/GameManagerData")]
public class GameManagerData : ScriptableObject
{
    // time delay before start game 
    public float GameReadyTime;

    // stage time 
    public float GameStageTime;

    // time delay before boss shows up 
    public float BossReadyTime;

    // time delay before game over 
    [Tooltip("게임 종료(사망, 클리어) 상태로 전화되기까지 걸리는 시간을 설정")]
    public float GameEndTimeDelay;
}
