using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    [SerializeField] private MagnetData _magnetData;
    private Transform playerTransform;
    public bool isMagnetize = true;

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void FixedUpdate()
    {
        if (isMagnetize && gameObject.CompareTag("EXPBox"))
        {
            transform.LookAt(playerTransform);
            transform.position = Vector3.MoveTowards(transform.position, playerTransform.position,
                _magnetData.fasterMagnetItemMoveSpeed * Time.deltaTime);

            isMagnetize = false;
        }
        else
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
}
