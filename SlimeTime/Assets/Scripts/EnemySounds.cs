using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySounds : MonoBehaviour
{
    public AudioClip horizontalWindupSound;
    public AudioClip verticalWindupSound;
    public AudioClip reflectWindupSound;
    public AudioClip smashSound;
    public AudioClip weaponSound;
    public AudioClip bluntAttackSound;
    public AudioClip enemyStaggeredSound;
    public AudioClip projectileLaunchSound;
    public AudioClip bossAttackSound;
    public AudioClip ghostTeleportSound;
    public AudioSource enemySoundSource;
    void Start()
    {
        if (enemySoundSource == null)
        {
            enemySoundSource = gameObject.AddComponent<AudioSource>();
            enemySoundSource.volume = 0.85f;
            enemySoundSource.loop = false;
        }
    }

    public void PlayVerticalWindupSound()
    {
        enemySoundSource.PlayOneShot(verticalWindupSound);
    }
    public void PlayHorizontalWindupSound()
    {
        enemySoundSource.PlayOneShot(horizontalWindupSound);
    }
    public void PlayReflectiveWindupSound()
    {
        enemySoundSource.PlayOneShot(reflectWindupSound);
    }

    public void PlaySmashSound()
    {
        enemySoundSource.PlayOneShot(smashSound);
    }

    public void PlaySlashSound()
    {
        enemySoundSource.PlayOneShot(weaponSound);
    }

    public void PlayBluntSound()
    {
        enemySoundSource.PlayOneShot(bluntAttackSound);
    }

    public void PlayBossAttackSound()
    {
        enemySoundSource.PlayOneShot(bossAttackSound);
    }
}
