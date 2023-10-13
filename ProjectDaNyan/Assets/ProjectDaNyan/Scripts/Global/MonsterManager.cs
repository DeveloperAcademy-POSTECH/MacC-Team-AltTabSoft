using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    private static MonsterManager inst = null;
    public static MonsterManager Inst { get { if (inst == null) { return null; } return inst; } }

    [SerializeField] List<Transform> monsterSpawnPoints;

    [SerializeField] GameObject monsterBossPrefab;

    MonsterNormalTypeFactory myMonsterNormalTypeFactory = null;

    float currentTime;

    GameState currentGameState;


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

        // delegate chain, add time and game state observer 
        GameManager.Inst.delegateTimeCount += OnCalledEverySecond;
        GameManager.Inst.delegateGameState += checkGameState;
    }


    public void checkGameState(GameState gameState)
    {
        currentGameState = gameState;

        switch (currentGameState)
        {
            case GameState.inGame:

                int spawnPosition = Random.Range(0, 4);

                MonsterNormal monster = null;

                // nornal short range type 
                if (currentTime % 3 == 0)
                {
                    monster = myMonsterNormalTypeFactory.Spawn(Monster_Normal.SHORT_RANGE);
                    monster.transform.position = monsterSpawnPoints[spawnPosition].transform.position;
                }

                // normal long range type
                if (currentTime % 6 == 0)
                {
                    monster = myMonsterNormalTypeFactory.Spawn(Monster_Normal.LONG_RANGE);
                    monster.transform.position = monsterSpawnPoints[spawnPosition].transform.position;
                }


                // elite type 
                if (currentTime % 20 == 0)
                {
                    monster = myMonsterNormalTypeFactory.Spawn(Monster_Normal.ELITE);
                    monster.transform.position = monsterSpawnPoints[spawnPosition].transform.position;
                }

            break;


            case GameState.bossStage:
                // do something
                GameObject boss = ObjectPoolManager.Inst.BringObject(monsterBossPrefab);
                boss.transform.position = monsterSpawnPoints[0].transform.position;

            break;

        }
    }

    // get game time from game manager 
    public void OnCalledEverySecond(float t)
    {
        currentTime = t;
    }

}
