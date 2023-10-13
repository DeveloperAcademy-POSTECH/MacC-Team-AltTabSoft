using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawnPoints : MonoBehaviour
{
    PlayerController Target;

    public GameObject target;

    private void Awake()
    {
        Target = GetComponent<PlayerController>();

    }


    // Update is called once per frame
    void Update()
    {
        this.transform.position = new Vector3(target.transform.position.x, 0, target.transform.position.z);
    }
}
