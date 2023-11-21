using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;

public class MonsterFatManCollider : MonsterAttack
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.gameObject.tag.Equals("Player") ||
            (other.gameObject.layer == LayerMask.NameToLayer("Map_Object") ||
             other.gameObject.layer == LayerMask.NameToLayer("Map_Floor")))
        {
            ObjectPoolManager.Inst.DestroyObject(this.gameObject);
        }
    }
}
