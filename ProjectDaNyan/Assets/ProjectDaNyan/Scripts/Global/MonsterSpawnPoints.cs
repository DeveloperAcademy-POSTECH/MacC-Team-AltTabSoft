using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawnPoints : MonoBehaviour
{
    public GameObject target;

    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player");
    }


    // Update is called once per frame
    void Update()
    {
        this.transform.position = new Vector3(target.transform.position.x, 1, target.transform.position.z);
    }
}
