using UnityEngine;


public class MonsterFatManCollider : MonsterAttack
{
    private void OnCollisionEnter(Collision other)
    {
        Debug.Log	($"Collider : {other.gameObject.name}");
        
        if (other.collider.gameObject.tag.Equals("Player") ||
            (other.gameObject.layer == LayerMask.NameToLayer("Map_Object") ||
             other.gameObject.layer == LayerMask.NameToLayer("Map_Floor")))
        {
            ObjectPoolManager.Inst.DestroyObject(this.gameObject);
        }
    }
}
