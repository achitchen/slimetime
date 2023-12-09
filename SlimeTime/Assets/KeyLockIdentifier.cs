using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyLockIdentifier : MonoBehaviour
{
    public int lockIdNumber;
    private ProgressScript progressScript;
    // Start is called before the first frame update
    void Start()

    {
        Invoke("ActivateLock", 0.05f);
    }

    private void ActivateLock()
    {
            progressScript = GameObject.Find("Game Manager").GetComponent<ProgressScript>();
            gameObject.SetActive(!progressScript.areLocksOpened[lockIdNumber]);
    }
}
