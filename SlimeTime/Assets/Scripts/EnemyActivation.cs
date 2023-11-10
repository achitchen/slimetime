using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActivation : MonoBehaviour
{
    [SerializeField] List<GameObject> enemies;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (enemies.Count != 0)
            {
                foreach(GameObject enemy in enemies)
                {
                    enemy.SetActive(true);
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
