using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public List<GameObject> enemies;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (enemies.Count != 0)
            {
                foreach(GameObject enemy in enemies)
                {
                    if (enemy.GetComponent<EnemyHealth>().assignedRoom == null)
                    {
                        enemy.GetComponent<EnemyHealth>().assignedRoom = gameObject;
                    }
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (enemies.Count != 0)
            {
                foreach (GameObject enemy in enemies)
                {
                    enemy.SetActive(false);
                }
            }
        }
    }
}
