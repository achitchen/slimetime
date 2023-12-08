using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchDoor : MonoBehaviour
{
    [SerializeField] GameObject lockedDoor;
    public bool isTriggered;
    private KeyManager keyManager;

    private void Start()
    {
        if (keyManager == null)
        {
            keyManager = GameObject.Find("Game Manager").GetComponent<KeyManager>();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isTriggered)
        {
            if (collision.gameObject.tag == "Player")
            {
                keyManager.switchDoorsOpened.Add(gameObject.transform.parent.gameObject);
                lockedDoor.GetComponent<DustParticleScript>().ActivateDust();
                lockedDoor.SetActive(false);
                isTriggered = true;
                gameObject.SetActive(false);
            }
        }
    }
}
