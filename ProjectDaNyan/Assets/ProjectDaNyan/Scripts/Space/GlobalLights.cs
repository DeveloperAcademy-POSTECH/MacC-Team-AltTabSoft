using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalLights : MonoBehaviour //GloabalLights 는 Player와 같은 하이어라키에 위치해 있어야 합니다.
{
    [SerializeField]
    private GameObject player;
    private Vector3 initPosition; 
    void Awake()
    {
        player = transform.parent.GetComponentInChildren<PlayerController>().gameObject;
        initPosition = transform.position - player.transform.position;
    }

    private void Update()
    {
        FollowPlayer();
    }

    private void FollowPlayer()
    {
        transform.position = initPosition + player.transform.position;
    }
}
