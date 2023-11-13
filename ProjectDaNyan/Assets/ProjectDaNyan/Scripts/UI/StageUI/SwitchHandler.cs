using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchHandler : MonoBehaviour
{
    [SerializeField] private Image on;
    [SerializeField] private Image off;
    private int _index = 0;

    public void OnSwitchClicked()
    {
        if (_index == 0)
        {
            on.gameObject.SetActive(false);
            off.gameObject.SetActive(true);
            _index = 1;
        }
        else
        {
            on.gameObject.SetActive(true);
            off.gameObject.SetActive(false);
            _index = 0;
        }
    }
}
