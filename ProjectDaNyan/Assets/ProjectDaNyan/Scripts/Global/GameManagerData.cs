using System.Collections;
using System.Collections.Generic;
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
}
