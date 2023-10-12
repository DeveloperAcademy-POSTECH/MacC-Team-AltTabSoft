using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public enum Type {Default, Piercing, Laser};
    public Type type;
    public int damage;
    TrailRenderer trail;


    int time = 3;

    private void OnEnable()
    {
        trail = GetComponent<TrailRenderer>();
        StartCoroutine(Goodbye());
    }

    private void Update()
    {
        //if (type != Type.Laser)
        //    Destroy(gameObject, 1f);

    }


    IEnumerator Goodbye()
    {


        //while(time > 0)
        //{
        //    time -= 1;
        //    yield return new WaitForSeconds(1);
        //}
        yield return new WaitForSeconds(1f);
        trail.Clear();
        ObejectPoolManager.Inst.DestroyObject(this.gameObject);
        

    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy" && type != Type.Piercing && type != Type.Laser) //관통 타입이 아니면 적에 맞을 경우 총알 오브젝트 사라
        {
            //적에 닿았을 때 로직
            Destroy(gameObject);
        }

        if (collision.gameObject.tag == "RandomObject" && type != Type.Laser) //태그명은 추후 변경 필요
        {
            //맵에 생성된 랜덤 오브젝트와 닿았을 때 사라
            Destroy(gameObject);
        }

    }
}
