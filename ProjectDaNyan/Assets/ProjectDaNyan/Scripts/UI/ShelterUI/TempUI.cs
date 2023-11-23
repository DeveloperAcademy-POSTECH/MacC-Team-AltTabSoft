using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempUI : MonoBehaviour
{
    private TempUI _tempUI;

    private void Awake()
    {
        _tempUI = FindObjectOfType<TempUI>();
    }

    public void OutOfTempUI()
    {
        _tempUI.gameObject.SetActive(false);
    }
}
