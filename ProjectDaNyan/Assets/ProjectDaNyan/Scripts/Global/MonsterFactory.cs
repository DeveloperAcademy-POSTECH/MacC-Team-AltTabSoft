using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

// create monster with requested monster type 
public abstract class MonsterFactory<T> : MonoBehaviour
{
    public MonsterNormal Spawn(T type)
    {
        MonsterNormal monster = this.GenerateMonster(type);

        return monster;
    }

    protected abstract MonsterNormal GenerateMonster(T type);
}
