using System.Collections;
using System.Collections.Generic;
using ProjectDaNyan.Scripts.UI.StageUI;
using UnityEngine;
using UnityEngine.UI;

public class SkillInfoUI : MonoBehaviour
{
    private SkillInfoUI _skillInfoUI;
    private GameObject _pauseUI;

    private void Awake()
    {
        _skillInfoUI = FindObjectOfType<SkillInfoUI>(includeInactive: true);
        _pauseUI = transform.parent.Find("PauseUI").gameObject;
    }

    public void GoToPauseUI()
    {
        _skillInfoUI.gameObject.SetActive(false);
        _pauseUI.SetActive(true);
    }
}
