using System;
using System.Collections;
using UnityEngine;


// game state machine 
public enum GameState
{
    readyGame,
    inGame,
    bossReady,
    bossStage,
    gameOver,
    win,
    resume,
}

// game manage check game state ( ingame, game over, game start, game play time) 

public class GameManager : MonoBehaviour
{
    //// 코드 확인 후 수정 예정
    ////===========================================================
    //#region // need to edit

    [SerializeField] private MapManager _mapManager;
    public bool isGameOver = false;

    //private void Awake()
    //{
    //    _mapManager = FindObjectOfType<MapManager>();
    //    Application.targetFrameRate = 120;
    //}

    //void Update()
    //{
    //    gameTime += Time.deltaTime;
    //    collectedCatBox = _mapManager.collectedCats;
    //    if (gameTime >= 120f) //(테스트용, 2분 뒤 GameOver)
    //    {
    //        isGameOver = true;
    //    }
    //}

    //#endregion
    ////===========================================================

    private static GameManager inst = null;
    public static GameManager Inst { get { if (inst == null) { return null; } return inst; } }


    public delegate void DelegateTimeCount(float t);
    public DelegateTimeCount delegateTimeCount;


    public delegate void DelegateGameState(GameState currentGameState);
    public DelegateGameState delegateGameState;

    //[SerializeField]private MapManager _mapManager;


   
    public float GameTime { get { return _currentTime; } }
    public GameState CurrentGameState { get { return _currentGameState; } }



    [Header("# Game Control")]
    [SerializeField] private float _monsterReGenTime;
    [SerializeField] private float _readyTime = 3;
    [SerializeField] private float _stageTime = 180f;
    [SerializeField] private float _currentTime = 0;
    [SerializeField] private float _bossReadyTime = 3;


    [Header("# Game Status")]
    [SerializeField] private GameState _currentGameState;


    [Header("# Player Info")]
    public int collectedCatBox;


    private void Awake()
    {
        // Singleton Pattern 
        #region Singleton Pattern
        if (inst == null)
        {
            inst = FindAnyObjectByType<GameManager>();

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
        ///

        Application.targetFrameRate = 120;

    }



    private void Start()
    {
        Debug.Log("manager start");
        _currentGameState = GameState.readyGame;
        StartCoroutine(idle());
    }


    // Game state machine 
    IEnumerator idle()
    {
        yield return new WaitForSeconds(0.1f);

        Debug.Log($"current game state {_currentGameState}");

        // notify 
        delegateGameState(_currentGameState);

        switch (_currentGameState)
        {
            case GameState.readyGame:
                StartCoroutine(readyGame());
            break;

            case GameState.inGame:
                StartCoroutine(inGame());
            break;

            case GameState.bossReady:
                StartCoroutine(bossReady());
            break;

            case GameState.bossStage:

            break;

            case GameState.gameOver:
                Debug.Log("Player is dead");
            break;

            case GameState.win:
                Debug.Log("Boss is dead");
            break;
        }
    }

    // do something before game start 
    IEnumerator readyGame()
    {
        while (_readyTime > 0)
        {
            // UI show game start countdown 

            if (delegateTimeCount != null)
            {
                delegateTimeCount(_readyTime);
            }

            Debug.Log($"ready : {_readyTime}");

            _readyTime -= 1;

            // wait for 1 second 
            yield return new WaitForSeconds(1);
        }

        _currentGameState = GameState.inGame;
        StartCoroutine(idle());
    }



    // do something during game playing 
    IEnumerator inGame()
    {
        yield return new WaitForSeconds(1);

        _currentTime += 1;
        delegateTimeCount(_currentTime);

        if (_currentTime == _stageTime)
        {
            _currentGameState = GameState.bossReady;
        }

        StartCoroutine(idle());
    }


    IEnumerator bossReady()
    {
        // wait until boss stage ready time 
        while (_bossReadyTime > 0)
        {
            Debug.Log("GameManager_Boss ready!");


            _bossReadyTime -= 1;
            yield return new WaitForSeconds(1);
        }

        _currentGameState = GameState.bossStage;
        StartCoroutine(idle());
    }


    // player is dead, you lose 
    public void PlayerDead()
    {
        _currentGameState = GameState.gameOver;
    }


    // boss is dead, player win
    public void BossDead()
    {
        Debug.Log($"boss dead {_currentGameState}");
        _currentGameState = GameState.win;
    }

    // pause game
    public void PauseGame()
    {
        Time.timeScale = 0;
    }


    // resume game 
    public void ResumeGame()
    {
        Time.timeScale = 1;
    }
}
