using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShelterUIScript : MonoBehaviour
{
    private Button _buttonExit;
    private Button _buttonStage;
    private GameObject _staticCanvas;
    private GameObject _lowerBar;
    private GameObject _lowerBox;
    private GameObject _upperBar;
    private GameObject _extraBottom;
    private GameObject _extraTop;
    private Image _blackScreen;
    private GameObject _transitionCanvas;

    // Start is called before the first frame update
    void Start()
    {
        _upperBar = GetComponentInChildren<UpperBar>().gameObject;
        _lowerBox = GetComponentInChildren<LowerBox>().gameObject;
        _extraTop = GetComponentInChildren<ExtraTop>().gameObject;
        _extraBottom = GetComponentInChildren<ExtraBottom>().gameObject;
        _blackScreen = GetComponentInChildren<BlackScreen>(includeInactive:true).gameObject.GetComponent<Image>();
        _transitionCanvas = GetComponentInChildren<TranstionCanvas>(includeInactive:true).gameObject;

        var buttons = GetComponentsInChildren<Button>();
        foreach (var button in buttons)
        {
            Debug.Log(button.transform.name);
            var buttonName = button.transform.name;
            if (buttonName == "Button_Exit")
            {
                _buttonExit = button;
            }
            else if (buttonName == "Button_Stage")
            {
                _buttonStage = button;
            }
        }

        _buttonStage.onClick.AddListener(() =>
        { 
            float endValue = 600f;  
            float duration = 0.5f;
            _transitionCanvas.SetActive(true);
            _blackScreen.DOFade(1f, duration*0.8f);
            _extraTop.transform.DOMoveY(_extraTop.transform.position.y + endValue, duration);
            _extraBottom.transform.DOMoveY(_extraBottom.transform.position.y - endValue, duration);
            _upperBar.transform.DOMoveY(_upperBar.transform.position.y + endValue, duration);
            _lowerBox.transform.DOMoveY(_lowerBox.transform.position.y - endValue, duration).OnComplete(() =>
            {
                SceneManager.LoadScene("StageScene");
            });
        });

        _buttonExit.onClick.AddListener(() => { Application.Quit(); });
    }
}