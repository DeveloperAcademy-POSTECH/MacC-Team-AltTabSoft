using TMPro;
using UnityEngine;

namespace ProjectDaNyan.Scripts.UI.StageUI
{
    public class CollectedBoxCatPauseText: MonoBehaviour // 게임 오버, 게인 클리어 때 나타나는 숫자
    {
        private TextMeshProUGUI _text;
        private PlayerStatus _playerStatus;
    
        private void Awake()
        {
            _text = GetComponent<TextMeshProUGUI>();
            _playerStatus = FindObjectOfType<PlayerStatus>();
        }

        private void OnEnable()
        {
            _text.text = string.Concat(_playerStatus.Player_collected_box_cat.ToString()," 고양이 구출");
        }
    }
}
