using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BGMSwitchHandler : MonoBehaviour
{
    [SerializeField] private Image on;
    [SerializeField] private Image off;
    private AudioSource _bgm;
    private int _switchIndex = 0;

    private void Awake()
    {
        _bgm = FindObjectOfType<AudioSource>();
    }

    public void OnSwitchClicked()
    {
        if (_switchIndex == 0)
        {
            on.gameObject.SetActive(false);
            off.gameObject.SetActive(true);
            _bgm.Pause();
            _switchIndex = 1;
        }
        else
        {
            on.gameObject.SetActive(true);
            off.gameObject.SetActive(false);
            _bgm.Play();
            _switchIndex = 0;
        }
    }
}
