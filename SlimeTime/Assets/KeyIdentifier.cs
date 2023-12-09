using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyIdentifier : MonoBehaviour
{
    public int keyIdNumber;
    private ProgressScript progressScript;
    // Start is called before the first frame update
    void Start()

    {
        Invoke("ActivateKey", 0.05f);
    }

    private void ActivateKey()
    {
        if (keyIdNumber != 0)
        {
            progressScript = GameObject.Find("Game Manager").GetComponent<ProgressScript>();
            gameObject.SetActive(!progressScript.areKeysCollected[keyIdNumber]);

        }
    }
}
