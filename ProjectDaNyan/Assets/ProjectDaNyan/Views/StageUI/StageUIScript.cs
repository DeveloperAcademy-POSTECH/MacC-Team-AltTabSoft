using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ProjectDaNyan.Scripts
{
    public class GameObjectTouch : MonoBehaviour
    {
        private Button _buttonGoToShelter;
        private Button _buttonPause;
        private GameObject _pauseUI;
        private GameObject _staticCanvas;
        private Button _buttonContinueStage;
        
        private Image _blackScreen;
        private GameObject _transitionCanvas;
        
        private void Awake()
        {
            _transitionCanvas = GetComponentInChildren<TranstionCanvas>(includeInactive:true).gameObject;
            _transitionCanvas.SetActive(true);
        }

        // Start is called before the first frame update
        void Start()
        {
            _pauseUI = transform.Find("PauseCanvas").gameObject;
            _staticCanvas = transform.Find("StaticCanvas").gameObject;
            _blackScreen = GetComponentInChildren<BlackScreen>(includeInactive:true).gameObject.GetComponent<Image>();
        
            var buttons = GetComponentsInChildren<Button>(includeInactive: true);
            foreach (var button in buttons)
            {
                var buttonName = button.transform.name;
                if (buttonName == "Button_GoTo_Shelter") _buttonGoToShelter = button;
                else if (buttonName == "Button_Pause") _buttonPause = button;
                else if (buttonName == "Button_Continue_Stage") _buttonContinueStage = button;
            }
        
            // 각 버튼 별 역할 세팅
            _buttonGoToShelter.onClick.AddListener(() =>
            {
                float duration = 0.5f;
                
                //화면 암전
                _transitionCanvas.SetActive(true);
                _blackScreen.DOFade(1f, duration*0.8f).OnComplete(() =>
                {
                    SceneManager.LoadScene("ShelterScene");
                });
            });
        
            _buttonPause.onClick.AddListener(() =>
            {
                _pauseUI.SetActive(true);
                _staticCanvas.SetActive(false);
            });
            
            _buttonContinueStage.onClick.AddListener(() =>
            {
                _pauseUI.SetActive(false);
                _staticCanvas.SetActive(true);
            });

            //화면 불러올 때
            _blackScreen.DOFade(0f, 0.2f).OnComplete(() =>
            {
                _transitionCanvas.SetActive(false);
            });
        }
    }
}
