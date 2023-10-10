using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public enum Type {Default, Piercing};
    public Type type;
    public int damage;

    private void Update()
    {
        Destroy(gameObject,2);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy" && type != Type.Piercing) //관통 타입이 아니면 적에 맞을 경우 총알 오브젝트 사라
        {
            //적에 닿았을 때 로직
            Destroy(gameObject);
        }

        if (collision.gameObject.tag == "RandomObject") //태그명은 추후 변경 필요
        {
            //맵에 생성된 랜덤 오브젝트와 닿았을 때 사라
            Destroy(gameObject);
        }

    }
}
