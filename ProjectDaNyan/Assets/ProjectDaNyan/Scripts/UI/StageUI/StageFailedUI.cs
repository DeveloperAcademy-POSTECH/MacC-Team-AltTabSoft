using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageFailedUI : MonoBehaviour
{
    private void OnEnable()
    {
        GameManager.Inst.PauseGame();
    }
}
