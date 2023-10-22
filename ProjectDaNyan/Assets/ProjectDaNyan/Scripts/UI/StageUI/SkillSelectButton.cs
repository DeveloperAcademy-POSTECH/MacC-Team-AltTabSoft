using UnityEngine;
using UnityEngine.UI;

namespace ProjectDaNyan.Scripts.UI.StageUI
{
    public class SkillSelectButton : MonoBehaviour
    {
        private SkillSelectUI _skillSelectUI;
        private PlayerStatus _playerStatus;
        private Button _button;
        

        private void Awake()
        {
            _playerStatus = FindObjectOfType<PlayerStatus>();
            _skillSelectUI = FindObjectOfType<SkillSelectUI>(includeInactive: true);
            _button = GetComponent<Button>();
        }
        public void SelectSkillBox()
        {
            var overFlowedExp = _playerStatus.Player_now_EXP - _playerStatus.Level_Up_Require_EXP ;
            _playerStatus.Player_now_EXP = overFlowedExp; // 레벨에 필요한 만큼만 경험치를 제외, 연속적인 레벨업 가능
            _skillSelectUI.gameObject.SetActive(false);
            GameManager.Inst.ResumeGame();
            _button.onClick.RemoveAllListeners();
        }
    }
}
