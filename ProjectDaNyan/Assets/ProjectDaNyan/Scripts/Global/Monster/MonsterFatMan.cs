using System.Collections;
using UnityEngine;

public class MonsterFatMan : MonsterAttack
{
    [SerializeField] private GameObject _explosion;
    [SerializeField] private GameObject _fatManBody;
    [SerializeField] private GameObject _waveBlast;
    [SerializeField] private Transform _blastPoint;
    [SerializeField] private float _explosionDamageDecrease;
    
    // bomb target prefab 
    [SerializeField] private GameObject _fatManTargetCircle;
    private void OnEnable()
    {
        _fatManBody.SetActive(true);

        StartCoroutine(MakeTargetMark());

    }


    IEnumerator MakeTargetMark()
    {
        yield return new WaitForSeconds(0.1f);
        
        GameObject target = ObjectPoolManager.Inst.BringObject(_fatManTargetCircle);
        target.transform.position = new Vector3(this.transform.position.x, 1, this.transform.position.z);
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.gameObject.tag.Equals("Player") || 
            (other.gameObject.layer == LayerMask.NameToLayer("Map_Object") || 
             other.gameObject.layer == LayerMask.NameToLayer("Map_Floor")))
        {
            
            ObjectPoolManager.Inst.DestroyObject(_fatManTargetCircle);
            _fatManBody.SetActive(false);
            
            StartCoroutine(fatManExplosion());
        }
        
    }

    
    

    IEnumerator fatManExplosion()
    {
        // generate exlposion particle effect 
        GameObject afterExplosion = ObjectPoolManager.Inst.BringObject(_explosion);
        afterExplosion.transform.position = this.gameObject.transform.position;

        GameObject blast = ObjectPoolManager.Inst.BringObject(_waveBlast);
        blast.transform.position = _blastPoint.position;
        MonsterSkillWaveBlast skill = blast.GetComponent<MonsterSkillWaveBlast>();

        skill.Damage = this.Damage * _explosionDamageDecrease;
        skill.StartWaveBlastAttack(SkillType.WaveBlast);

        yield return new WaitForSeconds(0.2f);

        ObjectPoolManager.Inst.DestroyObject(afterExplosion);

        yield return new WaitForSeconds(0.5f);
        ObjectPoolManager.Inst.DestroyObject(blast);

        yield return new WaitForSeconds(0.1f);
        ObjectPoolManager.Inst.DestroyObject(this.gameObject);
    }
}
