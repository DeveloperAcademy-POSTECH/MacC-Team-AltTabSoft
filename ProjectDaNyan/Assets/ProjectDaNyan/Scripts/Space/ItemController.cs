using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    private MapManager mapManager;

    private void Start()
    {
        mapManager = FindObjectOfType<MapManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            mapManager.collectedCats++;
            gameObject.SetActive(false);
            Debug.Log("획득한 고양이 수: " +  mapManager.collectedCats);
        }
    }
}

