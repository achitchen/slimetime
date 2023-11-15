using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyScript : MonoBehaviour
{
    public bool hasKey = false;
    public int keyCount = 0;
    private KeyManager keyManager;
    private GameObject lastKeyCollected;

    private void Start()
    {
        if(keyManager == null)
        {
            keyManager = GameObject.Find("Game Manager").GetComponent<KeyManager>();
        }
        lastKeyCollected = null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Key")
        {
            lastKeyCollected = collision.gameObject.transform.parent.gameObject;
            if (!hasKey)
            {
                hasKey = true;
            }
            keyCount++;

            collision.gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Lock")
        {
            if (hasKey)
            {
                keyManager.keyDoorsOpened.Add(collision.gameObject.transform.parent.gameObject);
                keyManager.keysFound.Add(lastKeyCollected);
                collision.gameObject.SetActive(false);
                
                keyCount--;
                if (keyCount == 0)
                {
                    hasKey = false;
                }
            }
        }
    }
}
