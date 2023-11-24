using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageClearUI : MonoBehaviour
{
    private void OnEnable()
    {
        GameManager.Inst.PauseGame();
    }
}
