using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using System.Text.RegularExpressions;

public class GameObjectPool : MonoBehaviour
{
    private IObjectPool<GameObject> pool;
    public IObjectPool<GameObject> Pool { get { return pool; } }

    public GameObject prefab = null;

    // remove () from name 
    string pattern = @" ?\(.*?\)";


    private void Awake()
    {
        pool = new ObjectPool<GameObject>(CreateNewObject, BringObjectFromPool,
            ReturnObjectToPool, DestroyObjectFromPool, true, 3, 5);
    }

     GameObject CreateNewObject()
    {
        GameObject myObject = Instantiate<GameObject>(prefab, this.transform);

        myObject.name = Regex.Replace(myObject.name, pattern, "");
        return myObject;
    }

    private void BringObjectFromPool(GameObject monster)
    {
        prefab = monster;
        monster.gameObject.SetActive(true);
        monster.transform.position = transform.parent.position;
    }

    private void ReturnObjectToPool(GameObject myObject)
    {
        myObject.transform.position = transform.parent.position;
        myObject.gameObject.SetActive(false);
    }

    private void DestroyObjectFromPool(GameObject myObject)
    {
        Destroy(myObject.gameObject);
    }
}
