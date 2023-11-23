using System.Collections;
using System.Collections.Generic;
using System;
using ProjectDaNyan.Views.StageUI;
using UnityEngine;
using TMPro;

public class CoolTimeText : MonoBehaviour
{
    private TextMeshProUGUI _text;
    private StageUIScript _stageUIScript;
    
    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
        _stageUIScript = FindObjectOfType<StageUIScript>();
    }
    
    private void LateUpdate()
    {
        float passedTime = _stageUIScript.HiddenLeftCoolTime;
        int min = Mathf.FloorToInt(passedTime / 60);
        int sec = Mathf.FloorToInt(passedTime % 60);
        _text.text = String.Format("{0:D2}:{1:D2}", min, sec);
    }
}
