using TMPro;
using UnityEngine;

namespace ProjectDaNyan.Scripts.UI.StageUI
{
    public class CollectedBoxCatText : MonoBehaviour // 인게임 실시간 상태에서
    {
        private TextMeshProUGUI _text;
        private PlayerStatus _playerStatus;
    
        private void Awake()
        {
            _text = GetComponent<TextMeshProUGUI>();
            _playerStatus = FindObjectOfType<PlayerStatus>();
        }

        private void LateUpdate()
        {
            _text.text = _playerStatus.Player_collected_box_cat.ToString();
        }
    }
}