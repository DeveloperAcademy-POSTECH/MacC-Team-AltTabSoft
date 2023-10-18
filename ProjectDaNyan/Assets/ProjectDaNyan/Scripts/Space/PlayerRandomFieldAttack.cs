using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRandomFieldAttack : MonoBehaviour
{
    public int randomFieldLevel = 1;
    public float fieldFireRate;
    public float randomRange;

    float randomFieldDelay;
    bool isRandomFieldReady;

    public GameObject attackField;
    public Transform center;

    public void UseRandomFieldAttack(bool isRandomField)
    {
        if (isRandomField)
        {
            isRandomField = fieldFireRate < randomFieldDelay;
            randomFieldDelay += Time.deltaTime;
            if (isRandomField)
            {
                StartCoroutine("RandomFieldAttack");
                randomFieldDelay = 0;
            }
            
        }
    }

    void MakeRandomAttackField(GameObject fieldObject, Transform fieldCenter)
    {
        GameObject attackField = ObjectPoolManager.Inst.BringObject(fieldObject);
        Vector3 randomVector = new Vector3(Random.Range(-randomRange, randomRange),0, Random.Range(-randomRange, randomRange));
        attackField.transform.position = fieldCenter.position + randomVector;

    }

    IEnumerator RandomFieldAttack()
    {
        yield return null;
        MakeRandomAttackField(attackField, center);

    }
}
