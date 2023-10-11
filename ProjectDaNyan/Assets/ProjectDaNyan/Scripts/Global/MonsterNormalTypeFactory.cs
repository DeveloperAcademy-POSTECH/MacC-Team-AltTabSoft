using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;


// normal monster type
public enum Monster_Normal
{
    SHORT_RANGE,
    LONG_RANGE,
    ELITE
}

public class MonsterNormalTypeFactory : MonsterFactory<Monster_Normal>
{
    // set monster prefabs 
    [SerializeField] private GameObject monsterShortRangePrefab = null;
    [SerializeField] private GameObject monsterLongRangePrefab = null;
    [SerializeField] private GameObject monsterElitePrefab = null;

    // request objectpoolmanager to bring prefab 
    protected override MonsterNormal GenerateMonster(Monster_Normal type)
    {

        MonsterNormal monster = null;

        switch (type)
        {
            case Monster_Normal.SHORT_RANGE:
                // bring object from pool
                monster = ObejectPoolManager.Inst.BringObject(monsterShortRangePrefab).GetComponent<MonsterNormal>();
                break;

            case Monster_Normal.LONG_RANGE:
                // bring object from pool
                monster = ObejectPoolManager.Inst.BringObject(monsterLongRangePrefab).GetComponent<MonsterNormal>(); ;
                break;

            case Monster_Normal.ELITE:
                // bring object from pool
                monster = ObejectPoolManager.Inst.BringObject(monsterElitePrefab).GetComponent<MonsterNormal>(); ;
                break;
        }


        return monster;

    }
}
