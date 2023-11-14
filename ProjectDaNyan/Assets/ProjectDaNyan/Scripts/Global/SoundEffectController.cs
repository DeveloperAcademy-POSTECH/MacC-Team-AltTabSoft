using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StageSoundEffectController : MonoBehaviour
{
    [SerializeField] public AudioClip[] soundEffects;
    [SerializeField] public AudioSource audioSource;

    private bool isSoundEffectPlayTemp = true;
    private float sampleVolumeSettingValue = 1f;

    public enum SoundTypes
    {
        stop = -1,
        Boss_Approach = 0,
        Boss_Clear = 1,
        Boss_Dash_Attack = 2,
        Boss_Entire_Attack = 3,
        Boss_Fight_Area = 4,
        Boss_Normal_Attack = 5,
        Boxcat_Destroy = 6,
        Boxcat_Drink = 7,
        Boxcat_Gold = 8,
        Boxcat_Rescue = 9,
        Level_Up = 10,
        Monster_Die = 11,
        Monster_Hit = 12,
        Player_Attack = 13,
        Player_Blackhole_Attack = 14,
        Player_Bombboom_Attack = 15,
        Player_Bombplant_Attack = 16,
        Player_Dash_0 = 17,
        Player_Dash_1 = 18,
        Player_Dash_Attack = 19,
        Player_Drone_Attack = 20,
        Player_Hidden_Allattack = 21,
        Player_Hidden_Hacking = 22,
        Player_Hidden_Heal = 23,
        Player_Hidden_Laser  = 24,
        Player_Object_Dash = 25,
        Player_Water_Walk = 26,
        Stage_Clear = 27
    }
    private SoundTypes soundType = SoundTypes.stop;

    public void playSoundEffect(float effectVolume, SoundTypes _type)
    {
        if (!isSoundEffectPlayTemp)
        {
            return;
        }
        
        if (_type != soundType)
        {
            soundType = _type;
            audioSource.volume = effectVolume * sampleVolumeSettingValue;
            audioSource.PlayOneShot(soundEffects[(int)soundType]);
        }
    }
}