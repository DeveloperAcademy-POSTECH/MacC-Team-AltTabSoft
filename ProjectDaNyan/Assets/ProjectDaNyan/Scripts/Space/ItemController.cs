using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ItemController : MonoBehaviour
{
    private static ItemController inst = null;
    public static ItemController Inst
    {
        get
        {
            if (inst == null)
            {
                // 인스턴스가 없을 경우, 현재 Scene에서 찾아봅니다.
                inst = FindObjectOfType<ItemController>();
            }
            return inst;
        }
    }

    private void Awake()
    {
        if (inst == null)
        {
            inst = this; // 인스턴스를 할당합니다.
        }
    }

    public void DropItem()
    {
        var random = Random.Range(0, 101);

        if (random <= 75)
        {
            DropGold();
        }
        else
        {
            DropEnergy();
        }
    }

    private void DropGold()
    { 
        var random = Random.Range(0, 101);

        if (random <= 20)
        {
            Debug.Log("LargeCoin 획득");
        }
        else if (random <= 50)
        {
            Debug.Log("MiddleCoin 획득");
        }
        else
        {
            Debug.Log("SmallCoin 획득");
        }
    }

    private void DropEnergy()
    {
        Debug.Log("Energy 획득");
    }
}

