using UnityEngine;
using UnityEngine.UI;

namespace ProjectDaNyan.Scripts.UI.StageUI
{
    public class PlayerLevelSlider : MonoBehaviour
    {
        [SerializeField] private PlayerData playerData;
        private PlayerStatus _playerStatus;
        private Slider _playerLevelSlider; 
        void Awake()
        {
            _playerStatus = FindObjectOfType<PlayerStatus>();
            _playerLevelSlider = GetComponent<Slider>();
        }
        // Update is called once per frame
        private void Update()
        {
            _playerLevelSlider.value = (float) _playerStatus.Player_now_EXP / 100;
        }
    }
}
