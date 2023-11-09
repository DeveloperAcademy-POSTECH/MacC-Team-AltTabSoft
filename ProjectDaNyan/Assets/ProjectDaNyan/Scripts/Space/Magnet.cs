using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    [SerializeField] private MagnetData _magnetData;
    private Transform playerTransform;

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void FixedUpdate()
    {
        float distance = Vector3.Distance(transform.position, playerTransform.position);

        if (distance <= _magnetData.magnetDistance)
        {
            transform.LookAt(playerTransform);
            transform.position = Vector3.MoveTowards(transform.position, playerTransform.position,
                _magnetData.magnetItemMoveSpeed * Time.deltaTime);
        }
    }
}
