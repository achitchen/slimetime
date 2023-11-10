using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyScript : MonoBehaviour
{
    public bool hasKey = false;
    public int keyCount = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Key")
        {
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
                collision.gameObject.GetComponent<LockedDoor>().unlockedDoor.SetActive(true);
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
