using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyScript : MonoBehaviour
{
    public bool hasKey = false;
    public int keyCount = 0;
    private GameManagerSounds gameManagerSounds;
    private PlayerSounds playerSounds;
    private ProgressScript progressScript;
    private GameObject keyUI;

    private void Start()
    {
        if (playerSounds == null)
        {
            playerSounds = GetComponent<PlayerSounds>();
        }
        if (gameManagerSounds == null)
        {
            gameManagerSounds = GameObject.Find("Game Manager").GetComponent<GameManagerSounds>();
        }
        if (progressScript == null)
        {
            progressScript = GameObject.Find("Game Manager").GetComponent<ProgressScript>();
        }
        keyCount = progressScript.keysPossessed;
        if (keyCount != 0)
        {
            hasKey = true;
        }
        if (keyUI == null)
        {
            keyUI = GameObject.Find("KeyImage");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Key")
        {
            playerSounds.playerSoundSource.PlayOneShot(playerSounds.keyPickup);
            if (!hasKey)
            {
                hasKey = true;
            }
            keyCount++;
            progressScript.keysPossessed++;
            progressScript.areKeysCollected[collision.gameObject.GetComponentInParent<KeyIdentifier>().keyIdNumber] = true;
            keyUI.GetComponent<KeyText>().UpdateText();

            collision.gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Lock")
        {
            if (hasKey)
            {
                gameManagerSounds.gameManagerSoundSource.PlayOneShot(gameManagerSounds.doorsUnlockedSound);
                progressScript.areLocksOpened[collision.gameObject.GetComponent<KeyLockIdentifier>().lockIdNumber] = true;
                collision.gameObject.GetComponent<DustParticleScript>().ActivateDust();
                collision.gameObject.SetActive(false);
                
                keyCount--;
                progressScript.keysPossessed--;
                keyUI.GetComponent<KeyText>().UpdateText();
                if (keyCount == 0)
                {
                    hasKey = false;
                }
            }
        }
    }
}
