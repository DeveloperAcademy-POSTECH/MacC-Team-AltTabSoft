using System;
using TMPro;
using UnityEngine;

public class UIInfoTime: MonoBehaviour
{
    private TextMeshProUGUI _text;
    
    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    private void LateUpdate()
    {
        float passedTime = GameManager.Inst._gameTime;
        int min = Mathf.FloorToInt(passedTime / 60);
        int sec = Mathf.FloorToInt(passedTime % 60);
        _text.text = String.Format("{0:D2} : {1:D2}", min, sec);
    }
}