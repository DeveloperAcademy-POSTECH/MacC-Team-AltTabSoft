using System.Collections;
using UnityEngine;

public class GameManager1 : MonoBehaviour
{
    private static GameManager1 inst = null;
    public static GameManager1 Inst { get { if (inst == null) { return null; } return inst; } }


    public delegate void DelegateTimeCount(float t);
    public DelegateTimeCount delegateTimeCount;


    //[SerializeField]private MapManager _mapManager;

    [Header("# Game Control")] public float gameTime;
    [SerializeField] float MonsterReGenTime;
    [SerializeField] float readyTime = 5;
    [SerializeField] float StageTime = 480f;
    [SerializeField] bool BossStage;
    [SerializeField] public GameState currentGameState;


    [Header("# Player Info")] public int collectedCatBox;

    public enum GameState
    {
        startGame,
        inGame,
        bossStage,
        gameOver,
        win
    }


    private void Awake()
    {
        // Singleton Pattern 
        #region Singleton Pattern
        if (inst == null)
        {
            inst = FindAnyObjectByType<GameManager1>();

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
        currentGameState = GameState.startGame;
        StartCoroutine(gameState());
    }


    // Game state machine 
    IEnumerator gameState()
    {

        while (StageTime > 0)
        {
            yield return StartCoroutine(currentGameState.ToString());
        }

        StartCoroutine(bossStage());
    }


    // do something before game start 
    IEnumerator startGame()
    {
        while (readyTime > 0)
        {
            // UI show game start countdown 


            readyTime = timer(readyTime);

            if (delegateTimeCount != null)
            {
                delegateTimeCount(readyTime);
            }

            Debug.Log($"ready : {readyTime}");

            // wait for 1 second 
            yield return new WaitForSeconds(1);
        }

        currentGameState = GameState.inGame;
    }


    // time counter 
    float timer (float t)
    {
        t -= 1;

        return t;
    }





    // do something during game playing 
    IEnumerator inGame()
    {

        //Debug.Log($"inGame {StageTime}");


        StageTime = timer(StageTime);

        delegateTimeCount(StageTime);

        if(StageTime >= 480)
        {
            currentGameState = GameState.bossStage;
        }

        yield return new WaitForSeconds(1);
    }



    // boss stage start 
    IEnumerator bossStage()
    {
        // create boss 

        yield return null;
    }


    // failed stage 
    IEnumerator gameOver()
    {



        yield return null;
    }


    // won stage 
    IEnumerator win()
    {



        yield return null;
    }

    /// <summary>
    /// Player HP == 0 -> game over 
    /// Boss HP == 0 -> game win 
    /// Time 0 -> 8 min
    /// at 8 min -> Boss -> time stop
    ///
    /// monster gen per 2 sec 
    /// </summary>






}
