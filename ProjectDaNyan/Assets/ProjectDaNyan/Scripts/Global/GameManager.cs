using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; //스태틱으로 선언된 변수는 인스펙터에서 감지되지 않음
    [SerializeField] private MapManager _mapManager;
    [Header("# Game Control")] public float gameTime;
    [Header("# Player Info")] public int collectedCatBox = 0;
    public bool isGameOver = false;

    private void Awake()
    {
        Instance = this;
        _mapManager = FindObjectOfType<MapManager>();
        Application.targetFrameRate = 120;
    }

    void Update()
    {
        gameTime += Time.deltaTime;
        collectedCatBox = _mapManager.collectedCats;
        if (gameTime >= 2f)
        {
            isGameOver = true;
        } 
    }
}
