using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public GameObject assignedDoor;
    void Start()
    {
        if (assignedDoor != null)
        {
            assignedDoor.GetComponent<CombatDoor>().roomEnemies.Add(gameObject);
        }
    }
}
