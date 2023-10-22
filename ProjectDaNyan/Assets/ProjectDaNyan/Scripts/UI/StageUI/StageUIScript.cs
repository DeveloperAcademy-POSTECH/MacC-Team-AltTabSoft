using System;
using System.Diagnostics;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

namespace ProjectDaNyan.Views.StageUI
{
    public class StageUIScript : MonoBehaviour
    {
        private Button _buttonGoToShelter;
        private Button _buttonPause;
        private GameObject _pauseUI;
        private GameObject _stageMainUI;
        private Button _buttonContinueStage;

        private Image _blackScreen;
        private GameObject _transitionCanvas;
        private GameObject _stageClearUI;
        private GameObject _stageFailedUI;
        private GameObject _skillSelectUI;

        public GameObject SkillSelectUI
        {
            get { return _skillSelectUI; }
        }

        private PlayerStatus _playerStatus;

        private void Awake()
        {
            _transitionCanvas = GetComponentInChildren<TranstionCanvas>(includeInactive: true).gameObject;
            _transitionCanvas.SetActive(true);
            _playerStatus = FindObjectOfType<PlayerStatus>().gameObject.GetComponent<PlayerStatus>();
            Debug.Log(_playerStatus);
        }

        // Start is called before the first frame update
        void Start()
        {
            _pauseUI = transform.Find("PauseUI").gameObject;
            _stageClearUI = transform.Find("StageClearUI").gameObject;
            _stageFailedUI = transform.Find("StageFailedUI").gameObject;
            _stageMainUI = transform.Find("StageMainUI").gameObject;
            _skillSelectUI = transform.Find("SkillSelectUI").gameObject;
            _blackScreen = GetComponentInChildren<BlackScreen>(includeInactive: true).gameObject.GetComponent<Image>();

            var buttons = GetComponentsInChildren<Button>(includeInactive: true); // 버튼별 역할 할당, 각 버튼별로 스크립트 편집 예정
            foreach (var button in buttons)
            {
                var buttonName = button.transform.name;
                if (buttonName == "Button_GoTo_Shelter")
                {
                    button.onClick.AddListener(() =>
                    {
                        float duration = 0.5f;

                        //화면 암전
                        _transitionCanvas.SetActive(true);
                        _blackScreen.DOFade(1f, duration * 0.8f)
                            .SetUpdate(true) // TimeScale 값에 무관하게 동작
                            .OnComplete(() =>
                        {
                            GameManager.Inst.ResumeGame();
                            SceneManager.LoadScene("ShelterScene");
                            
                        });
                    });
                }
                else if (buttonName == "PauseButton")
                {
                    button.onClick.AddListener(() =>
                    {
                        _pauseUI.SetActive(true);
                        _stageMainUI.SetActive(false);
                        GameManager.Inst.PauseGame();
                    });
                }
                else if (buttonName == "Button_Continue_Stage")
                {
                    button.onClick.AddListener(() =>
                    {
                        _pauseUI.SetActive(false);
                        _stageMainUI.SetActive(true);
                        GameManager.Inst.ResumeGame();
                    });
                }
                
                else if (buttonName == "Button_Restart_Stage")
                {
                    button.onClick.AddListener(() =>
                    {
                        GameManager.Inst.ResumeGame();
                        SceneManager.LoadScene("StageScene");
                    });
                }
            }

            //화면 불러올 때
            _blackScreen.DOFade(0f, 0.2f).OnComplete(() => { _transitionCanvas.SetActive(false); });
        }

        private void Update()
        {
            switch (GameManager.Inst.CurrentGameState)
            {
                case GameState.readyGame:
                    break;
                case GameState.inGame:
                    break;
                case GameState.bossReady:
                    break;
                case GameState.bossStage:
                    break;
                case GameState.gameOver:
                    GameManager.Inst.PauseGame();
                    if (ReferenceEquals(_stageClearUI, null) == false)
                        _stageClearUI.SetActive(false); // serialized, public 변수는 null 체크를 이렇게 하면 안됨
                    if (ReferenceEquals(_stageFailedUI, null) == false) _stageFailedUI.SetActive(true);
                    break;
                case GameState.win:
                    GameManager.Inst.PauseGame();
                    if (ReferenceEquals(_stageClearUI, null) == false)
                        _stageClearUI.SetActive(true); // serialized, public 변수는 null 체크를 이렇게 하면 안됨
                    if (ReferenceEquals(_stageFailedUI, null) == false) _stageFailedUI.SetActive(false);
                    break;
                case GameState.resume:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }


            if (_playerStatus.Level_Up_Require_EXP - _playerStatus.Player_now_EXP <= 0)
            {
                GameManager.Inst.PauseGame();
                _skillSelectUI.SetActive(true);
            }
        }
    }
}