using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchDoor : MonoBehaviour
{
    [SerializeField] GameObject lockedDoor;
    public bool isTriggered;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isTriggered)
        {
            if (collision.gameObject.tag == "Player")
            {
                lockedDoor.SetActive(false);
                isTriggered = true;
            }
        }
    }
}
