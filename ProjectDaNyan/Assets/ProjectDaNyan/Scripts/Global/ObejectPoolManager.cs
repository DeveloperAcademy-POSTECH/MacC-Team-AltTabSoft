using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class ObejectPoolManager : MonoBehaviour
{
    //  dictionary 
    static Dictionary<string, GameObject> gameObjectPools = new Dictionary<string, GameObject>();

    [SerializeField] GameObject gameObjectPool;

    private static ObejectPoolManager inst = null;
    public static ObejectPoolManager Inst { get { if (inst == null) { return null; } return inst; } }

    // rename pool name  
    string pattern = @"Prefab";

    private void Awake()
    {
        // Singleton Pattern 
        #region Singleton Pattern
        if (inst == null)
        {
            inst = FindAnyObjectByType<ObejectPoolManager>();

            if (inst == null)
            {
                inst = this;

                DontDestroyOnLoad(this);
            }
        }
        else
        {
            Destroy(this.gameObject);
        }
        #endregion
        //// Singleton Pattern
    }


    public GameObject BringObject(GameObject targetObject)
    {
        string key = targetObject.name.ToString();

        GameObject newObject = null;

        // check if requested object has its own pool, if not, make new pool 
        if (!gameObjectPools.ContainsKey(key))
        {
            createNewPool(targetObject);
        }

        // Bring object from pool by key 
        newObject = gameObjectPools[key].GetComponent<GameObjectPool>().Pool.Get();


        return newObject;
    }

    // return object to pool 
    public void DestroyObject(GameObject destroyObject)
    {
        string key = destroyObject.name.ToString();

        // check if requested object has its own pool, if not, make new pool 
        if (!gameObjectPools.ContainsKey(key))
        {
            createNewPool(destroyObject);
        }

        // return object to pool by key 
        gameObjectPools[key].GetComponent<GameObjectPool>().Pool.Release(destroyObject);
    }


    // create new pool
    void createNewPool(GameObject targetObject)
    {
        string key = targetObject.name.ToString();

        // create new pool 
        GameObject newPool = Instantiate<GameObject>(gameObjectPool, this.transform);
        newPool.GetComponent<GameObjectPool>().prefab = targetObject;
        // rename the pool 
        newPool.name = $"Pool_{Regex.Replace(key, pattern, "")}";
        // add pool to dictionary 
        gameObjectPools.Add(key, newPool);
    }
}
