using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatDoor : MonoBehaviour
{
    public List<GameObject> roomEnemies;
    [SerializeField] GameObject lockedDoor;
    private bool isUnlocked = false;
    private float doorCountDownFloat = 0.2f;

    private void Update()
    {
        if (!isUnlocked)
        {
            if (roomEnemies.Count == 0)
            {
                StartCoroutine("DoorCountDown");
            }
        }
    }

    private IEnumerator DoorCountDown()
    {
        yield return new WaitForSeconds(doorCountDownFloat);
        lockedDoor.SetActive(false);
        isUnlocked = true;
    }
}
