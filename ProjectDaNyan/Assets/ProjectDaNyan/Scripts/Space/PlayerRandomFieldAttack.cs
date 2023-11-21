using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRandomFieldAttack : MonoBehaviour
{
    [SerializeField] private AttackStatus _attackStatus;
    [SerializeField] private float _fieldFireRate;
    [SerializeField] private float _randomRange;
    [SerializeField] private GameObject attackField;
    [SerializeField] private Transform center;
    public int randomFieldLevel = 1;
    private float _randomFieldDelay;

    private void OnEnable()
    {
        _fieldFireRate = _attackStatus.fieldFireRate;
        _randomRange = _attackStatus.randomRange;
    }

    public void UseRandomFieldAttack(bool isRandomField)
    {
        if (isRandomField)
        {
            isRandomField = _fieldFireRate < _randomFieldDelay;
            _randomFieldDelay += Time.deltaTime;
            if (isRandomField)
            {
                StartCoroutine("RandomFieldAttack");
                _randomFieldDelay = 0;
            }
            
        }
    }

    void MakeRandomAttackField(GameObject fieldObject, Transform fieldCenter)
    {
        GameObject attackField = ObjectPoolManager.Inst.BringObject(fieldObject);
        Vector3 randomVector = new Vector3(Random.Range(-_randomRange, _randomRange),0, Random.Range(-_randomRange, _randomRange));
        attackField.transform.position = fieldCenter.position + randomVector;

    }

    IEnumerator RandomFieldAttack()
    {
        yield return null;
        MakeRandomAttackField(attackField, center);
    }
}
