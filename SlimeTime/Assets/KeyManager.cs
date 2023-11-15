using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyManager : MonoBehaviour
{
    public List<GameObject> keysFound;
    public List<GameObject> keyDoorsOpened;
    public List<GameObject> switchDoorsOpened;

    /*private void Start()
    {
        foreach (GameObject key in keysFound)
        {
            if (key != null && key.activeSelf)
            {
                key.SetActive(false);
            }
        }
        foreach (GameObject keyDoor in keyDoorsOpened)
        {
            if (keyDoor != null && keyDoor.activeSelf)
            {
                keyDoor.SetActive(false);
            }
        }
        foreach (GameObject switchDoor in switchDoorsOpened)
        {
            if (switchDoor != null && switchDoor.activeSelf)
            {
                switchDoor.SetActive(false);
            }
        }
    }*/
}
