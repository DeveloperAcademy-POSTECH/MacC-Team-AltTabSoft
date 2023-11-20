using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class SoundEffectController : MonoBehaviour
{
    [SerializeField] public AudioSource audioSource;
    
    public bool isSoundEffectPlayTemp = true;
    private float sampleVolumeSettingValue = 1f;

    private AudioClip[] StageSoundEffects;
    private AudioClip[] ShelterUISoundEffects;
    
    public enum StageSoundTypes
    {
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
        Stage_Clear = 27,
    }

    public enum ShelterUISoundTypes
    {
        Shelter_Player_Interaction = 0,
        Shelter_UI_Battle_Start = 1,
        UI_Menu_Button = 2
    }
    
    private void Start()
    {
        StageSoundEffects = Resources.LoadAll<AudioClip>("Effect_Stage");
        ShelterUISoundEffects = Resources.LoadAll<AudioClip>("Effect_Shelter&UI");
    }

    public void playStageSoundEffect(float effectVolume, StageSoundTypes _type)
    {
        if (!isSoundEffectPlayTemp)
        {
            return;
        }
        audioSource.volume = effectVolume * sampleVolumeSettingValue;
        audioSource.PlayOneShot(StageSoundEffects[(int)_type]);
    }
    
    public void playShelterUISoundEffects(float effectVolume, ShelterUISoundTypes _type)
    {
        if (!isSoundEffectPlayTemp)
        {
            return;
        }
        audioSource.volume = effectVolume * sampleVolumeSettingValue;
        audioSource.PlayOneShot(ShelterUISoundEffects[(int)_type]);
    }
}