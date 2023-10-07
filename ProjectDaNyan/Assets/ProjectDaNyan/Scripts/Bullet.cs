using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
   
    public int damage;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            //적에 닿았을 때 로직
            Destroy(gameObject);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "RandomObject")
        {
            //맵에 생성된 랜덤 오브젝트와 닿았을 때 사라
            Destroy(gameObject);
        }
    }
}
