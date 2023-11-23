using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShelterUIScript : MonoBehaviour
{
    private Button _buttonExit;
    private Button _buttonStage;
    private Button _buttonPlayer;
    private Button _buttonSkill;
    private Button _buttonExitCharacterInfoUI;
    private Button _buttonGotoWeaponInfoUI;
    private Button _buttonExitWeaponInfoUI;
    
    private GameObject _staticCanvas;
    private GameObject _lowerBar;
    private GameObject _lowerBox;
    private GameObject _upperBar;
    // private GameObject _extraBottom;
    // private GameObject _extraTop;
    private Image _blackScreen;
    private GameObject _transitionCanvas;
    private RectMask2D _canvasSafeAreaRectMask2D;
    
    private GameObject _characterInfoUI;
    private GameObject _weaponInfoUI;
    private GameObject _tempUI;
    private GameObject _settingUI;

    // Start is called before the first frame update
    void Start()
    {
        //GameManager timeScale 호출이 안됨??
        Time.timeScale = 1;
        
        _canvasSafeAreaRectMask2D = GetComponentInChildren<CanvasSafeArea>().gameObject.GetComponent<RectMask2D>();
        _upperBar = GetComponentInChildren<UpperBar>().gameObject;
        _lowerBox = GetComponentInChildren<LowerBox>().gameObject;
        // _extraBottom = GetComponentInChildren<ExtraBottom>().gameObject;
        // _extraTop = GetComponentInChildren<ExtraTop>().gameObject;
        _blackScreen = GetComponentInChildren<BlackScreen>(includeInactive:true).gameObject.GetComponent<Image>();
        
        _transitionCanvas = GetComponentInChildren<TranstionCanvas>(includeInactive:true).gameObject;
        
        // ChildUIs
        _characterInfoUI = GetComponentInChildren<CharacterInfoUI>(includeInactive: true).gameObject;
        _weaponInfoUI = GetComponentInChildren<WeaponInfoUI>(includeInactive: true).gameObject;
        _settingUI = GetComponentInChildren<SettingUI>(includeInactive: true).gameObject;
        _tempUI = GetComponentInChildren<TempUI>(includeInactive: true).gameObject;
        
        //화면 불러올 때
        _transitionCanvas.SetActive(true);
        _blackScreen.color =  new Color(_blackScreen.color.r, _blackScreen.color.g, _blackScreen.color.b, 1f);
        _blackScreen.DOFade(0f, 0.4f).OnComplete(() =>
        {
            _transitionCanvas.SetActive(false);
        });
        
        var buttons = GetComponentsInChildren<Button>(includeInactive: true);
        foreach (var button in buttons)
        {
            var buttonName = button.transform.name;
            if (buttonName == "Button_Exit") // 게임 종료
            {
                button.onClick.AddListener(() =>
                {
                    Application.Quit(); 
                });
            } else if (buttonName == "Button_Setting") // 설정 창 불러오기
            {
                button.onClick.AddListener(() =>
                {
                    _settingUI.SetActive(true);
                });
            } else if (buttonName == "Button_Exit_SettingUI") // 설정 창 닫기
            {
                button.onClick.AddListener(() =>
                {
                    _settingUI.SetActive(false);
                });
            }
            else if (buttonName == "StageButton") //스테이지 불러오기
            {
                button.onClick.AddListener(() =>
                { 
                    float endValue = 600f;  
                    float duration = 0.5f;
            
                    //화면 암전
                    _transitionCanvas.SetActive(true);
                    _blackScreen.DOFade(1f, duration*0.8f);
            
                    //UI 바깥으로 이동
                    _canvasSafeAreaRectMask2D.enabled = false;
                    // _extraBottom.transform.DOMoveY(_extraBottom.transform.position.y + endValue, duration);
                    // _extraTop.transform.DOMoveY(_extraTop.transform.position.y - endValue, duration);
                    _upperBar.transform.DOMoveY(_upperBar.transform.position.y + endValue, duration);
                    _lowerBox.transform.DOMoveY(_lowerBox.transform.position.y - endValue, duration).OnComplete(() =>
                    {
                        SceneManager.LoadScene("StageScene");
                    });
                });
            }
            else if (buttonName != "TempUI")
            {
                button.onClick.AddListener(() =>
                {
                    _tempUI.SetActive(true);
                });
            }
            // else if (buttonName == "CharacterInfoButton") // 캐릭터 정보창 열기
            // {
            //     button.onClick.AddListener(() =>
            //     {
            //         _characterInfoUI.SetActive(true);
            //     });
            // } else if (buttonName == "Button_ExitCharacterInfoUI") // 캐릭터 정보창 닫기
            // {
            //     button.onClick.AddListener(() =>
            //     {
            //         _characterInfoUI.SetActive(false);
            //     });
            // } else if (buttonName == "Button_GoTo_WeaponInfoUI") // 무기 정보창 열기
            // {
            //     button.onClick.AddListener(() =>
            //     {
            //         _weaponInfoUI.SetActive(true);
            //     });
            // } else if (buttonName == "Button_ExitWeaponInfoUI") // 무기 정보창 닫기
            // {
            //     button.onClick.AddListener(() =>
            //     {
            //         _weaponInfoUI.SetActive(false);
            //     });
            // }
        }
    }
}