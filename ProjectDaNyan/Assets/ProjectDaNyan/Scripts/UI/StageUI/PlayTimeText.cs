using System;
using TMPro;
using UnityEngine;

namespace ProjectDaNyan.Scripts.UI.StageUI
{
    public class PlayTimeText: MonoBehaviour
    {
        private TextMeshProUGUI _text;
    
        private void Awake()
        {
            _text = GetComponent<TextMeshProUGUI>();
        }

        private void LateUpdate()
        {
            float passedTime = GameManager.Inst.GameTime;
            int min = Mathf.FloorToInt(passedTime / 60);
            int sec = Mathf.FloorToInt(passedTime % 60);
            _text.text = String.Format("{0:D2}:{1:D2}", min, sec);
        }
    }
}