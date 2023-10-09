using UnityEngine;
using UnityEngine.UI;

public class WeaponDescriptionUI : MonoBehaviour
{
    private Button _buttonExitWeaponDescriptionUI;
    private void Start()
    {
        var buttons = GetComponentsInChildren<Button>(includeInactive: true);
        foreach (var button in buttons) // 버튼 컴포넌트 연결
        {
            // Debug.Log(button.transform.name);
            var buttonName = button.transform.name;
            if (buttonName == "Button_ExitWeaponDescriptionUI")
            {
                _buttonExitWeaponDescriptionUI = button;
            }
        }
        //무기 정보 창 닫기
        _buttonExitWeaponDescriptionUI.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
        });
    }
}
