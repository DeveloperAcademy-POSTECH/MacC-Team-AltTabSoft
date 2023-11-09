using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private CinemachineVirtualCamera _playerVCam;
    private CinemachineVirtualCamera _monsterVCam;
    public CinemachineVirtualCamera MonsterVCam
    {
        get { return _monsterVCam; }
        set { _monsterVCam = value; }
    }

    private void Awake()
    {
        CinemachineVirtualCamera[] cinemachineVirtualCameras = GetComponentsInChildren<CinemachineVirtualCamera>();
        foreach (CinemachineVirtualCamera virtualCamera in cinemachineVirtualCameras)
        {
            // Debug.Log($"{virtualCamera.name}");
            // Debug.Log($"{virtualCamera.name.Contains("Boss")}");
            if (virtualCamera.name.Contains("Boss"))
            {
                _monsterVCam = virtualCamera;
            }
            else
            {
                _playerVCam = virtualCamera;
            }
        }
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        _playerVCam.Follow = player.transform;
    }
}
