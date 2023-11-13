using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectController : MonoBehaviour
{
    [SerializeField] public AudioClip[] soundEffects;
    [SerializeField] public AudioSource audioSource;
    private int audiocounter = 0;

    public enum SoundTypes
    {
        sample = 0
    }
    private int _soundTypes; // 필요하다면 _score = 123; 꼴로 초깃값 직접 설정

    public int soundTypes
    {
        get
        {
            return soundTypes;
        }
        // Setter 블록
        set
        {
            if(_soundTypes != value)
            {
                audioSource.PlayOneShot(soundEffects[0]);
            }
            _soundTypes = value;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        audiocounter += 1;
        if (audiocounter >= 5)
        {
            audioSource.PlayOneShot(soundEffects[0]);
            audiocounter = 0;
        }
    }
}