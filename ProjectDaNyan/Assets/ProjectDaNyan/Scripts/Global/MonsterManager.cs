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

    public static MonsterManager Inst { get { return inst; } }

    [SerializeField] private List<Transform> _monsterSpawnPoints;

    [SerializeField] private GameObject _monsterBossAPrefab;
    [SerializeField] private GameObject _monsterBossBPrefab;
    [SerializeField] private GameObject _myObjectPool;


    [SerializeField] private MonsterManagerData _monsterManagerData;
    private MonsterNormalTypeFactory _monsterNormalTypeFactory = null;


    private int _spawnPosition;
    private float _timePasses;
    private float _currentTime;
    private GameState _currentGameState;

    // monster manage data;
    [SerializeField] private float _spawnInterval;
    [SerializeField] private int _totalSpawnQty;
    [SerializeField] private int _normalShortRangeQty;
    [SerializeField] private int _normalLongRangeQty;
    [SerializeField] private int _eliteShortRangeQty;
    [SerializeField] private int _eliteLongRangeQty;
    [SerializeField] private BossType _bossMonsterType;


    private void Awake()
    {
        inst = this;

        _myObjectPool = Resources.Load<GameObject>("GameObjectPool");

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

        _monsterNormalTypeFactory = GetComponent<MonsterNormalTypeFactory>();

        // delegate chain, add time and game state observer 
        GameManager.Inst.delegateTimeCount += OnCalledEverySecond;

        GameManager.Inst.delegateGameState += CheckGameState;
    }


    private void generateMonster()
    {
        if (_timePasses >= _spawnInterval)
        {
            _timePasses = 0;
        }
    }


    private void spawnBossMonster()
    {
        Vector3 genPos = _monsterSpawnPoints[1].transform.position;

        switch (_bossMonsterType)
        {
            case BossType.BossA:
                GameObject bossA = ObjectPoolManager.Inst.BringObject(_monsterBossAPrefab);
                bossA.transform.position = new Vector3(genPos.x, 1.5f, genPos.z);
                break;


            case BossType.BossB:
                GameObject bossB = ObjectPoolManager.Inst.BringObject(_monsterBossBPrefab);
                bossB.transform.position = new Vector3(genPos.x, 1.5f, genPos.z);
                break;
        }
    }


    private void spawnMonsters()
    {
        if (_timePasses >= _spawnInterval)
        {
            _timePasses = 0;

            _spawnPosition = Random.Range(0, 4);


            // total monster spanw quantity 
            _totalSpawnQty = _normalLongRangeQty + _normalShortRangeQty + _eliteLongRangeQty + _eliteShortRangeQty;

            StartCoroutine(spawnNormalShortMonster(_normalShortRangeQty));
            StartCoroutine(spawnNormalLongMonster(_normalLongRangeQty));

            StartCoroutine(spawnEliteShortMonster(_eliteShortRangeQty));
            StartCoroutine(spawnEliteLongMonster(_eliteLongRangeQty));
        }
    }


    // ================================ monster spawn coroutines
    #region monster spawn coroutines 
    IEnumerator spawnNormalShortMonster(int qty)
    {
        MonsterNormal monster = null;

        while (qty > 0)
        {
            qty--;

            monster = _monsterNormalTypeFactory.Spawn(Monster_Normal.NormalShortRange);

            monster.transform.position = _monsterSpawnPoints[_spawnPosition].transform.position + Vector3.forward * qty;
        }

        yield return null;
    }

    IEnumerator spawnNormalLongMonster(int qty)
    {
        MonsterNormal monster = null;

        while (qty > 0)
        {
            qty--;

            monster = _monsterNormalTypeFactory.Spawn(Monster_Normal.NormalLongRange);

            monster.transform.position = _monsterSpawnPoints[_spawnPosition].transform.position + Vector3.left * qty;
        }

        yield return null;
    }

    IEnumerator spawnEliteShortMonster(int qty)
    {
        MonsterNormal monster = null;

        while (qty > 0)
        {
            qty--;

            monster = _monsterNormalTypeFactory.Spawn(Monster_Normal.EliteShortRange);

            monster.transform.position = _monsterSpawnPoints[_spawnPosition].transform.position + Vector3.back * qty;
        }

        yield return null;
    }

    IEnumerator spawnEliteLongMonster(int qty)
    {
        MonsterNormal monster = null;

        while (qty > 0)
        {
            qty--;

            monster = _monsterNormalTypeFactory.Spawn(Monster_Normal.EliteLongRange);
            monster.transform.position = _monsterSpawnPoints[_spawnPosition].transform.position + Vector3.right * qty;
        }

        yield return null;
    }
    #endregion
    // ================================ monster spawn coroutines 


    // get game time from game manager 
    public void OnCalledEverySecond(float t)
    {
        // count time when inGame state only 
        if (_currentGameState == GameState.inGame)
        {
            _currentTime = t;
            _timePasses++;

            spawnMonsters();
        }
    }


    // called when game state is changed 
    public void CheckGameState(GameState gameState)
    {
        _currentGameState = gameState;

        if (_currentGameState == GameState.bossStage)
        {
            spawnBossMonster();
        }
    }
}