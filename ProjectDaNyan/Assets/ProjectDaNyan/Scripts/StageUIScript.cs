using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ProjectDaNyan.Scripts
{
    public class StageUIScript : MonoBehaviour
    {
        private Button _buttonGoToShelter;
        private Button _buttonPause;
        private GameObject _pauseUI;
        private GameObject _staticCanvas;
        private Button _buttonContinueStage;

        // Start is called before the first frame update
        void Start()
        {
            _pauseUI = transform.Find("PauseCanvas").gameObject;
            _staticCanvas = transform.Find("StaticCanvas").gameObject;
        
            var buttons = GetComponentsInChildren<Button>(includeInactive: true);
            foreach (var button in buttons)
            {
                Debug.Log(button.transform.name);
                var buttonName = button.transform.name;
                if (buttonName == "Button_GoTo_Shelter") _buttonGoToShelter = button;
                else if (buttonName == "Button_Pause") _buttonPause = button;
                else if (buttonName == "Button_Continue_Stage") _buttonContinueStage = button;
            }
        
            // 각 버튼 별 역할 세팅
            _buttonGoToShelter.onClick.AddListener(() =>
            {
                SceneManager.LoadScene("ShelterScene");
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
        }
    }
}
