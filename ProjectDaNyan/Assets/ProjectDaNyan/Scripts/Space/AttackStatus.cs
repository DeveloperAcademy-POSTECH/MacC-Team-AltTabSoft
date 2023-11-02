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
    public float bombFireRate;
    public float fieldFireRate;

    //Speed of Bullet
    public float basicFireSpeed;
    public float upgradedFireSpeed;
    public float droneFireSpeed;
    public float bombFireSpeed;

    //Speed of Drone
    public float droneSpeed;

    //Scan Range
    public int playerScanRange;
    public int droneScanRange;

    //Range of Random Field Attack
    public float randomRange;
}
