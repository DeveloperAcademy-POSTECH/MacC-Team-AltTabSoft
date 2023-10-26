using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum BossType
{
    BossA,
    BossB
}

public class MonsterManager : MonoBehaviour
{
    private static MonsterManager inst = null;
    public static MonsterManager Inst { get { if (inst == null) { return null; } return inst; } }

    [SerializeField] private List<Transform> monsterSpawnPoints;

    [SerializeField] private GameObject monsterBossAPrefab;
    [SerializeField] private GameObject monsterBossBPrefab;

    [SerializeField] private MonsterManagerData _monsterManagerData;
    private MonsterNormalTypeFactory _monsterNormalTypeFactory = null;


    private float timePasses;
    private float currentTime;
    private GameState currentGameState;

    // monster manage data;
    private float _spawnInterval;
    private float _totalSpawnQty;
    private float _normalShortRangeQty;
    private float _normalLongRangeQty;
    private float _eliteShortRangeQty;
    private float _eliteLongRangeQty;
    private BossType _bossMonsterType;


    private void Awake()
    {
        inst = this;
        _spawnInterval = _monsterManagerData.SpawnInterval;
        _totalSpawnQty = _monsterManagerData.TotalSpawnQty;
        _normalShortRangeQty = _monsterManagerData.NormalShortRangeQty;
        _normalLongRangeQty = _monsterManagerData.NormalLongRangeQty;
        _eliteShortRangeQty = _monsterManagerData.EliteShortRangeQty;
        _eliteLongRangeQty = _monsterManagerData.EliteLongRangeQty;
        _bossMonsterType = _monsterManagerData.BossMonsterType;
    }


    private void Start()
    {
        Debug.Log("monster on enable");

        _monsterNormalTypeFactory = GetComponentInChildren<MonsterNormalTypeFactory>();

        // delegate chain, add time and game state observer 
        GameManager.Inst.delegateTimeCount += OnCalledEverySecond;
        GameManager.Inst.delegateGameState += CheckGameState;
    }



    private void generateMonster()
    {

        if(timePasses >= _spawnInterval)
        {
            timePasses = 0;







        }




    }





    public void CheckGameState(GameState gameState)
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
                    monster = _monsterNormalTypeFactory.Spawn(Monster_Normal.NormalShortRange);
                    monster.transform.position = monsterSpawnPoints[spawnPosition].transform.position;
                }

                // normal long range type
                if (currentTime % 6 == 0)
                {
                    monster = _monsterNormalTypeFactory.Spawn(Monster_Normal.NormalLongRange);
                    monster.transform.position = monsterSpawnPoints[spawnPosition].transform.position;
                }


                // elite type 
                if (currentTime % 20 == 0)
                {
                    monster = _monsterNormalTypeFactory.Spawn(Monster_Normal.EliteLongRange);
                    monster.transform.position = monsterSpawnPoints[spawnPosition].transform.position;
                }

                break;


            case GameState.bossStage:
                // do something
                GameObject boss = ObjectPoolManager.Inst.BringObject(monsterBossAPrefab);
                Vector3 genPos = monsterSpawnPoints[1].transform.position;
                boss.transform.position = new Vector3(genPos.x, 1.5f, genPos.z);


            break;

        }
    }

    // get game time from game manager 
    public void OnCalledEverySecond(float t)
    {
        currentTime = t;
        timePasses++;
    }

}
