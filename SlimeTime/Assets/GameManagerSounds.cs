using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerSounds : MonoBehaviour
{
    public AudioClip playerDeathSound;
    public AudioClip bgmSound;
    public AudioClip checkpointSound;
    public AudioClip doorsLockedSound;
    public AudioClip doorsUnlockedSound;
    public AudioSource gameManagerSoundSource;
    public AudioSource bgmSoundSource;
    // Start is called before the first frame update
    void Start()
    {
        if (gameManagerSoundSource == null)
        {
            gameManagerSoundSource = gameObject.AddComponent<AudioSource>();
            gameManagerSoundSource.volume = 0.6f;
            gameManagerSoundSource.loop = false;
        }
        if (bgmSoundSource == null)
        {
            bgmSoundSource = gameObject.AddComponent<AudioSource>();
            bgmSoundSource.volume = 0.5f;
            bgmSoundSource.loop = true;
        }
        if (bgmSoundSource.clip == null)
        {
            bgmSoundSource.clip = bgmSound;
        }
    }
}
