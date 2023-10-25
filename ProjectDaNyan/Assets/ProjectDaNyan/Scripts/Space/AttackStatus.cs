using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackData", menuName = "Scriptable_Object/AttackData")]
public class AttackStatus : ScriptableObject
{
    //Time length between fire and the next fire
    public float basicFireRate;
    public float upgradedFireRate;
    public float laserFireRate;
    public float droneFireRate;
    public float fieldFireRate;

    //Speed of Bullet
    public float basicFireSpeed;
    public float upgradedFireSpeed;
    public float droneFireSpeed;

    //Speed of Drone
    public float droneSpeed;

    //Range of Random Field Attack
    public float randomRange;
}
