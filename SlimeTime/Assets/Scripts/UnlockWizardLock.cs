using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockWizardLock : MonoBehaviour
{
    [SerializeField] GameObject wizardLock;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            wizardLock.GetComponent<WizardLock>().OpenWizardDoor();
        }
    }
}
