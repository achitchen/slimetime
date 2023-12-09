using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardLock : MonoBehaviour
{
    public bool coreDestroyed1;
    public bool coreDestroyed2;
    public bool coreDestroyed3;
    public bool canBeUnlocked;
    public bool isUnlocked;
    private GameManager gameManager;
    [SerializeField] GameObject lockObject;
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        coreDestroyed1 = gameManager.coreDestroyed1;
        coreDestroyed2 = gameManager.coreDestroyed2;
        coreDestroyed3 = gameManager.coreDestroyed3;
        isUnlocked = false;
    }

    public void OpenWizardDoor()
    {
        if (!isUnlocked)
        {
            coreDestroyed1 = gameManager.coreDestroyed1;
            coreDestroyed2 = gameManager.coreDestroyed2;
            coreDestroyed3 = gameManager.coreDestroyed3;
            if (coreDestroyed1 && coreDestroyed2 && coreDestroyed3)
            {
                lockObject.SetActive(false);
                GameObject.Find("Game Manager").GetComponent<GameManagerSounds>().gameManagerSoundSource.PlayOneShot(GameObject.Find("Game Manager").GetComponent<GameManagerSounds>().doorsUnlockedSound);
            }
        }
    }
}
