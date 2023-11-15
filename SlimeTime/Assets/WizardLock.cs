using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardLock : MonoBehaviour
{
    public bool coreOneDestroyed;
    public bool coreTwoDestroyed;
    public bool coreThreeDestroyed;
    public bool canBeUnlocked;
    public bool isUnlocked;
    private GameManager gameManager;
    [SerializeField] GameObject lockObject;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        coreOneDestroyed = gameManager.coreOneDestroyed;
        coreTwoDestroyed = gameManager.coreTwoDestroyed;
        coreThreeDestroyed = gameManager.coreThreeDestroyed;
        isUnlocked = false;
    }

    public void OpenWizardDoor()
    {
        if (!isUnlocked)
        {
            if (coreOneDestroyed && coreTwoDestroyed && coreThreeDestroyed)
            {
                lockObject.SetActive(false);
            }
        }
    }
}
