using UnityEngine;
using UnityEngine.UI;

namespace ProjectDaNyan.Scripts.UI.StageUI
{
    public class PlayerExpSlider : MonoBehaviour
    {
        private GameObject _player;
        private PlayerStatus _playerStatus;
        private Slider _playerHpSlider; 

        private void Awake()
        {
            _player = GameObject.Find("Player");
            _playerStatus = FindObjectOfType<PlayerStatus>();
            _playerHpSlider = GetComponent<Slider>();
        }

        private void FixedUpdate()
        {
            _playerHpSlider.value = (float)_playerStatus.Player_Now_HP / _playerStatus.Player_Max_HP;
            Move();
        }

        private void Move()
        {
            _playerHpSlider.transform.position =
                Camera.main.WorldToScreenPoint(_player.transform.position + new Vector3(0, 4.5f, 0));
        }
    }
}
