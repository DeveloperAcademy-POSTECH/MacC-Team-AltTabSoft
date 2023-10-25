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
    private static GameManager inst = null;
    public static GameManager Inst { get { if (inst == null) { return null; } return inst; } }


    // delegate chain
    // share game time 
    public delegate void DelegateTimeCount(float t);
    public DelegateTimeCount delegateTimeCount;

    // share game status 
    public delegate void DelegateGameState(GameState currentGameState);
    public DelegateGameState delegateGameState;

    // share game time & game status 
    public float GameTime { get { return _currentTime; } }
    public GameState CurrentGameState { get { return _currentGameState; } }


    [Header("# Game Control")]
    [SerializeField] private GameManagerData _gameManagerData;


    [Header("# Game Status")]
    [SerializeField] private GameState _currentGameState;

    [Header("# Player Info")]
    public int collectedCatBox;


    // game time 
    private float _gameReadyTime = 3;
    private float _gameStageTime = 180f;
    private float _currentTime = 0;
    private float _bossReadyTime = 3;

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

        _gameReadyTime = _gameManagerData.GameReadyTime;
        _gameStageTime = _gameManagerData.GameStageTime;
        _bossReadyTime = _gameManagerData.BossReadyTime;
    }



    private void Start()
    {
        _currentGameState = GameState.readyGame;
        StartCoroutine(idle());
    }


    // Game state machine 게임 상태 체크용도
    IEnumerator idle()
    {
        yield return new WaitForSeconds(0.1f);

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
            break;

            case GameState.win:
            break;
        }
    }

    // do something before game start 
    IEnumerator readyGame()
    {
        while (_gameReadyTime > 0)
        {
            // UI show game start countdown 

            if (delegateTimeCount != null)
            {
                delegateTimeCount(_gameReadyTime);
            }
            _gameReadyTime -= 1;

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

        if (_currentTime == _gameStageTime)
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
