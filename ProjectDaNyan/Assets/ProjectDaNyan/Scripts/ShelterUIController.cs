using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace ProjectDaNyan.Scripts
{
    public class ShelterUIController : MonoBehaviour
    {
        private Button _openStage;
        // Start is called before the first frame update
    
        void Start()
        {
            var root = GetComponent<UIDocument>().rootVisualElement;
            _openStage = root.Q<Button>("OpenStage");
            
            _openStage.RegisterCallback<ClickEvent>(evt =>
            {
                Debug.Log("Hello World");
                SceneManager.LoadScene("StageScene");
            });
        }
    }
}
