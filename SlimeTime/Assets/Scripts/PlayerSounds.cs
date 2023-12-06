using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    public AudioClip dodgeSound;
    public AudioClip dodgeSuccess;
    public AudioClip deathSound;
    public AudioClip slimeShieldActivation;
    public AudioClip slimeShieldPop;
    public AudioClip keyPickup;
    public AudioClip riposteSound;

    public AudioSource playerSoundSource;

    void Start()
    {
        if (playerSoundSource == null)
        {
            playerSoundSource = gameObject.AddComponent<AudioSource>();
            playerSoundSource.volume = 0.7f;
            playerSoundSource.loop = false;
        }
    }

}
