using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{

    private static MonsterManager inst = null;
    public static MonsterManager Inst { get { if (inst == null) { return null; } return inst; } }


    [SerializeField] List<Transform> monsterSpawnPoints;

    MonsterNormalTypeFactory myMonsterNormalTypeFactory = null;

    float currentTime; 


    enum monsterType
    {
       normal,
       elite,
       boss
    }

    private void Start()
    {

        // Singleton Pattern 
        #region Singleton Pattern
        if (inst == null)
        {
            inst = FindAnyObjectByType<MonsterManager>();

            if (inst == null)
            {
                inst = this;

                DontDestroyOnLoad(this);
            }
        }
        else
        {
            Destroy(this.gameObject);
        }
        #endregion
        //// Singleton Pattern


        Debug.Log("monster on enable");

        myMonsterNormalTypeFactory = GetComponentInChildren<MonsterNormalTypeFactory>();


        GameManager1.Inst.delegateTimeCount += OnCalledEverySecond;
    }




    public void OnCalledEverySecond(float t)
    {
        // if game state is not in ingame 
        if(GameManager1.Inst.currentGameState != GameManager1.GameState.inGame)
        {
            return;
        }    

        currentTime = t;

        int spawnPosition = 3; //Random.Range(0, 4);

        //Debug.Log($"Monster Manager Current TIme : {currentTime}");

        MonsterNormal monster = null;

        // nornal short range type 
        if (t % 3 == 0)
        {
            monster = myMonsterNormalTypeFactory.Spawn(Monster_Normal.SHORT_RANGE);
            monster.transform.position = monsterSpawnPoints[spawnPosition].transform.position;
        }

        // normal long range type
        if(t % 6 == 0)
        {
            monster = myMonsterNormalTypeFactory.Spawn(Monster_Normal.LONG_RANGE);
            monster.transform.position = monsterSpawnPoints[spawnPosition].transform.position;
        }


        // elite type 
        if(t % 20 == 0)
        {
            monster = myMonsterNormalTypeFactory.Spawn(Monster_Normal.ELITE);
            monster.transform.position = monsterSpawnPoints[spawnPosition].transform.position;
        }


        // boss monster 
        if(t >= 480)
        {
            Debug.Log("Boss!!");
        }

    }


}
