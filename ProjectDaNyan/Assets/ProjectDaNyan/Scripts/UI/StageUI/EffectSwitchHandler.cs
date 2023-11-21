using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectSwitchHandler : MonoBehaviour
{
    [SerializeField] private Image on;
    [SerializeField] private Image off;
    private SoundEffectController _soundEffectController;
    private int _switchIndex = 0;

    private void Awake()
    {
        _soundEffectController = FindObjectOfType<SoundEffectController>();
    }

    public void OnSwitchClicked()
    {
        if (_switchIndex == 0)
        {
            on.gameObject.SetActive(false);
            off.gameObject.SetActive(true);
            _soundEffectController.isSoundEffectPlayTemp = false;
            _switchIndex = 1;
        }
        else
        {
            on.gameObject.SetActive(true);
            off.gameObject.SetActive(false);
            _soundEffectController.isSoundEffectPlayTemp = true;
            _switchIndex = 0;
        }
    }
}
