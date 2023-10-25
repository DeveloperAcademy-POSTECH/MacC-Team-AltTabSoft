using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;


// normal monster type
public enum Monster_Normal
{
    NormalShortRange,
    NormalLongRange,
    EliteLongRange,
    EliteShortRange

}

public class MonsterNormalTypeFactory : MonsterFactory<Monster_Normal>
{
    // set monster prefabs 
    [SerializeField] private GameObject monsterShortRangePrefab = null;
    [SerializeField] private GameObject monsterLongRangePrefab = null;
    [SerializeField] private GameObject monsterEliteLongRangePrefab = null;
    [SerializeField] private GameObject monsterEliteShortRangePrefab = null;


    // request objectpoolmanager to bring prefab 
    protected override MonsterNormal GenerateMonster(Monster_Normal type)
    {

        MonsterNormal monster = null;

        switch (type)
        {
            case Monster_Normal.NormalShortRange:
                // bring object from pool
                monster = ObjectPoolManager.Inst.BringObject(monsterShortRangePrefab).GetComponent<MonsterNormal>();
                break;

            case Monster_Normal.NormalLongRange:
                // bring object from pool
                monster = ObjectPoolManager.Inst.BringObject(monsterLongRangePrefab).GetComponent<MonsterNormal>(); ;
                break;

            case Monster_Normal.EliteLongRange:
                // bring object from pool
                monster = ObjectPoolManager.Inst.BringObject(monsterEliteLongRangePrefab).GetComponent<MonsterNormal>(); ;
                break;

            case Monster_Normal.EliteShortRange:
                // bring object from pool
                monster = ObjectPoolManager.Inst.BringObject(monsterEliteShortRangePrefab).GetComponent<MonsterNormal>(); ;
                break;
        }

        return monster;

    }
}
