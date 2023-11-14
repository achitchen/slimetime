using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActivation : MonoBehaviour
{
    [SerializeField] GameObject room;
    public List<GameObject> doorLocks;
    private bool isUnlocked = false;
    private float doorCountDownFloat = 0.2f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (room.GetComponent<EnemyManager>().enemies.Count != 0)
            {
                foreach (GameObject doorLock in doorLocks){
                    doorLock.SetActive(true);
                }
                foreach (GameObject enemy in room.GetComponent<EnemyManager>().enemies)
                {
                    enemy.SetActive(true);
                    if (enemy.GetComponent<EyeStateScript>() != null)
                    {
                        enemy.GetComponent<EyeStateScript>().resetBools();
                    }
                }
            }
        }
    }
    private void Update()
    {
        if (!isUnlocked)
        {
            if (room.GetComponent<EnemyManager>().enemies.Count == 0)
            {
                StartCoroutine("DoorCountDown");
            }
        }
    }

    private IEnumerator DoorCountDown()
    {
        yield return new WaitForSeconds(doorCountDownFloat);
        foreach (GameObject doorLock in doorLocks)
        {
            doorLock.SetActive(false);
        }
        isUnlocked = true;
    }
}