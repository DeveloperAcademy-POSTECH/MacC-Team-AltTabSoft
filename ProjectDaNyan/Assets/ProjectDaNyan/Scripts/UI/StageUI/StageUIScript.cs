using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

        private void Awake()
        {
            _transitionCanvas = GetComponentInChildren<TranstionCanvas>(includeInactive: true).gameObject;
            _transitionCanvas.SetActive(true);
        }

        // Start is called before the first frame update
        void Start()
        {
            _pauseUI = transform.Find("PauseUI").gameObject;
            _stageClearUI = transform.Find("StageClearUI").gameObject;
            _stageMainUI = transform.Find("StageMainUI").gameObject;
            _blackScreen = GetComponentInChildren<BlackScreen>(includeInactive: true).gameObject.GetComponent<Image>();

            var buttons = GetComponentsInChildren<Button>(includeInactive: true);
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
                        _blackScreen.DOFade(1f, duration * 0.8f).OnComplete(() =>
                        {
                            SceneManager.LoadScene("ShelterScene");
                        });
                    });
                }
                else if (buttonName == "Button_Pause")
                {
                    button.onClick.AddListener(() =>
                    {
                        _pauseUI.SetActive(true);
                        _stageMainUI.SetActive(false);
                    });
                }
                else if (buttonName == "Button_Continue_Stage")
                {
                    button.onClick.AddListener(() =>
                    {
                        _pauseUI.SetActive(false);
                        _stageMainUI.SetActive(true);
                    });
                }
            }

            //화면 불러올 때
            _blackScreen.DOFade(0f, 0.2f).OnComplete(() => { _transitionCanvas.SetActive(false); });
        }

        private void Update()
        {
            if (GameManager.Inst.isGameOver)
            {
                _stageClearUI.SetActive(true);
            }
        }
    }
}