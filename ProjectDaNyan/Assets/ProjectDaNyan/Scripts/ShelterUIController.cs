using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace ProjectDaNyan.Scripts
{
    public class ShelterUIController : MonoBehaviour
    {
        private Button _openStage;
        private Button _exitGame;
    
        void Start()
        {
            var root = GetComponent<UIDocument>().rootVisualElement;
            _openStage = root.Q<Button>("OpenStage");
            _exitGame = root.Q<Button>("ExitGame");
            
            _openStage.RegisterCallback<ClickEvent>(evt =>
            {
                SceneManager.LoadScene("StageScene");
            });
            
            _exitGame.RegisterCallback<ClickEvent>(evt =>
            {
                Debug.Log("Exit Application111");
                Application.Quit();
            });
        }
    }
}
