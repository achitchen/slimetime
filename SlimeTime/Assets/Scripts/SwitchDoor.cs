using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchDoor : MonoBehaviour
{
    [SerializeField] int switchIdNumber;
    [SerializeField] GameObject lockedDoor;
    private ProgressScript progressScript;
    public bool isTriggered;

    private void Start()
    {
        Invoke("InitialiseSwitch", 0.1f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isTriggered)
        {
            if (collision.gameObject.tag == "Player")
            {
                lockedDoor.GetComponent<DustParticleScript>().ActivateDust();
                lockedDoor.SetActive(false);
                isTriggered = true;
                gameObject.SetActive(false);
            }
        }
    }

    private void InitialiseSwitch()
    {
        if (progressScript == null)
        {
            progressScript = GameObject.Find("Game Manager").GetComponent<ProgressScript>();
        }
        isTriggered = progressScript.areSwitchesPressed[switchIdNumber];
        lockedDoor.SetActive(!progressScript.areSwitchesPressed[switchIdNumber]);
        gameObject.SetActive(!progressScript.areSwitchesPressed[switchIdNumber]);
    }
}
