using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActivation : MonoBehaviour
{
    [SerializeField] GameObject room;
    public List<GameObject> doorLocks;
    public List<GameObject> enemyList;
    private bool isUnlocked = false;
    private bool isActivated;
    private float doorCountDownFloat = 0.2f;

    private void Start()
    {
        isActivated = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            enemyList = room.GetComponent<EnemyManager>().enemies;
            if (!isActivated)
            {
                if (enemyList.Count != 0)
                {
                    foreach (GameObject doorLock in doorLocks)
                    {
                        doorLock.SetActive(true);
                        isActivated = true;
                    }
                    foreach (GameObject enemy in enemyList)
                    {
                        if (enemy != null)
                        {
                            enemy.SetActive(true);
                            if (enemy.GetComponent<EyeStateScript>() != null)
                            {
                                enemy.GetComponent<EyeStateScript>().Invoke("resetBools", 0.1f);
                            }
                            else if (enemy.GetComponent<ImpState>() != null)
                            {
                                enemy.GetComponent<ImpState>().Invoke("resetBools", 0.1f);
                            }
                            else if (enemy.GetComponent<WeaponImp>() != null)
                            {
                                enemy.GetComponent<WeaponImp>().Invoke("resetBools", 0.1f);
                            }
                            else if (enemy.GetComponent<GhostState>() != null)
                            {
                                enemy.GetComponent<GhostState>().Invoke("resetBools", 0.1f);
                            }
                            else if (enemy.GetComponent<CyclopsState>() != null)
                            {
                                enemy.GetComponent<CyclopsState>().Invoke("resetBools", 0.1f);
                            }
                        }
                        else
                        {
                            enemyList.Remove(enemy);
                        }
                    }
                }
            }
        }
            
    }
    private void Update()
    {
        if (isActivated)
        {
            if (!isUnlocked)
            {
                if (room != null)
                {
                    enemyList = room.GetComponent<EnemyManager>().enemies;
                    if (enemyList.Count == 0)
                    {
                        StartCoroutine("DoorCountDown");
                    }
                    else
                    {
                        foreach (GameObject enemy in enemyList.ToArray())
                        {
                            if (enemy == null)
                            {
                                enemyList.Remove(enemy);
                            }
                        }
                    }
                }
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
