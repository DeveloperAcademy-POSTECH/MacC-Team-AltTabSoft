using System.Collections;
using UnityEngine;
using static GameManager;


public enum GameState
{
    readyGame,
    inGame,
    bossReady,
    bossStage,
    gameOver,
    win,
    restart,
    endGame
}

public class GameManager : MonoBehaviour //게임 상태 모니터링 (예: 게임 중, 게임 오버, 게임 시작, 시간)
{
    private static GameManager inst = null;
    public static GameManager Inst { get { if (inst == null) { return null; } return inst; } }


    public delegate void DelegateTimeCount(float t);
    public DelegateTimeCount delegateTimeCount;


    public delegate void DelegateGameState(GameState currentGameState);
    public DelegateGameState delegateGameState;

    //[SerializeField]private MapManager _mapManager;

    [Header("# Game Control")] public float gameTime;
    [SerializeField] float monsterReGenTime;
    [SerializeField] float readyTime = 5;
    [SerializeField] float stageTime = 480f;
    [SerializeField] float currentTime = 0;
    [SerializeField] float bossReadyTime = 3;
    [SerializeField] public GameState CurrentGameState { get { return CurrentGameState; } }


    GameState currentGameState;


    [Header("# Player Info")] public int collectedCatBox;


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
                StartCoroutine(readyGame());
                break;

            case GameState.inGame:
                StartCoroutine(inGame());
                break;

            case GameState.bossReady:

                StartCoroutine(bossReady());
                break;

            case GameState.bossStage:
                StartCoroutine(bossStage());
                break;

            case GameState.gameOver:
                StartCoroutine(gameOver());
                break;

            case GameState.win:
                StartCoroutine(win());
                break;

            // re-load scene 
            case GameState.restart:
                StartCoroutine(restart());

                break;

            // end stage, load shelter scene 
            case GameState.endGame:

                StartCoroutine(endGame());
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

        while(bossReadyTime > 0)
        {
            // let UI show boss ready panel



            bossReadyTime -= 1;
            yield return new WaitForSeconds(1);
        }

        currentGameState = GameState.bossStage;
        StartCoroutine(idle());
    }



    // boss stage start 
    IEnumerator bossStage()
    {
        // create boss 

        yield return null;

        StartCoroutine(idle());
    }


    // failed stage 
    IEnumerator gameOver()
    {
        yield return null;

        StartCoroutine(idle());
    }


    // won stage 
    IEnumerator win()
    {
        // 시간이 7분 시간 되었을 때 && 보스를 잡았을 경우
        yield return null;

        StartCoroutine(idle());
    }

    IEnumerator endGame()
    {



        yield return null;
    }


    IEnumerator restart()
    {



        yield return null;
    }



    public void playerDead()
    {
        // 

        currentGameState = GameState.gameOver;

    }

    public void BossDead()
    {
        //

        currentGameState = GameState.win;
    }


}
