using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ItemController : MonoBehaviour
{
    // PlayerStatus에서 ItemController에 접근할 수 있도록
    private static ItemController inst = null;
    public static ItemController Inst
    {
        get
        {
            if (inst == null)
            {
                // 인스턴스가 없을 경우, 현재 Scene에서 찾기
                inst = FindObjectOfType<ItemController>();
            }
            return inst;
        }
    }

    private void Awake()
    {
        if (inst == null)
        {
            inst = this; // 인스턴스 할당
        }
    }

    public void DropItem()
    {
        // 구조 고양이, 골드, 드링크 등장 확률
        var randomCat = Random.Range(0, 101);
        var randomGold = Random.Range(0, 101);
        var randomDrink = Random.Range(0, 101);

        // 각 확률값에 따른 아이템 드랍
        DropCat(randomCat);
        DropGold(randomGold);
        DropDrink(randomDrink);
    }

    private void DropCat(int random)
    {
        if (random <= 75)
        {
            Debug.Log("구조 고양이 획득");
        }
    }

    private void DropGold(int random)
    { 
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

    private void DropDrink(int random)
    {
        if (random <= 20)
        {
            Debug.Log("Drink 획득");
        }
    }
}

