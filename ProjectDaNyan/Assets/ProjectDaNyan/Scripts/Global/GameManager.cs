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

    [Header("# Game Control")] public float gameTime;
    [SerializeField] float monsterReGenTime;
    [SerializeField] float readyTime = 3;
    [SerializeField] float stageTime = 60f;
    [SerializeField] float currentTime = 0;
    [SerializeField] float bossReadyTime = 3;
    [SerializeField] public GameState CurrentGameState { get { return currentGameState; } }


    Coroutine currentCoroutine;
    GameState currentGameState;
    GameState lastGameState;

    bool isPaused = false;

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
        currentGameState = GameState.readyGame;
        StartCoroutine(idle());
    }


    // Game state machine 
    IEnumerator idle()
    {
        yield return new WaitForSeconds(0.1f);

        Debug.Log($"current game state {currentGameState}");

        // notify 
        delegateGameState(currentGameState);

        switch (currentGameState)
        {
            case GameState.readyGame:
                currentCoroutine = StartCoroutine(readyGame());
                break;

            case GameState.inGame:
                currentCoroutine = StartCoroutine(inGame());
                break;

            case GameState.bossReady:

                currentCoroutine = StartCoroutine(bossReady());
                break;

            case GameState.bossStage:

                break;

            case GameState.gameOver:

                break;

            case GameState.win:

                break;
        }
    }

    // do something before game start 
    IEnumerator readyGame()
    {
        while (readyTime > 0)
        {
            // UI show game start countdown 

            if (delegateTimeCount != null)
            {
                delegateTimeCount(readyTime);
            }

            Debug.Log($"ready : {readyTime}");

            readyTime -= 1;

            // wait for 1 second 
            yield return new WaitForSeconds(1);
        }

        currentGameState = GameState.inGame;
        StartCoroutine(idle());
    }



    // do something during game playing 
    IEnumerator inGame()
    {
        yield return new WaitForSeconds(1);

        currentTime += 1;
        delegateTimeCount(currentTime);

        if (currentTime == stageTime)
        {
            currentGameState = GameState.bossReady;
        }

        StartCoroutine(idle());
    }


    IEnumerator bossReady()
    {

        // wait until boss stage ready time 
        while (bossReadyTime > 0)
        {
            Debug.Log("GameManager_Boss ready!");


            bossReadyTime -= 1;
            yield return new WaitForSeconds(1);
        }

        currentGameState = GameState.bossStage;
        StartCoroutine(idle());
    }


    // player is dead, you lose 
    public void PlayerDead()
    {
        currentGameState = GameState.gameOver;
    }


    // boss is dead, player win
    public void BossDead()
    {
        currentGameState = GameState.win;
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
