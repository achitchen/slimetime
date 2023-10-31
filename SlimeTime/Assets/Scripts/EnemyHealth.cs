using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHits = 3;
    public int currentHits = 0;
    public bool canBeRiposted = false;
    public bool isStaggered = false;
    [SerializeField] float staggerDuration = 2.5f;

    void Start()
    {
        currentHits = 0;
        canBeRiposted = false;
    }

    public void Death()
    {
        if (gameObject.GetComponent<DoorController>().assignedDoor != null)
        {
            gameObject.GetComponent<DoorController>().assignedDoor.GetComponent<CombatDoor>().roomEnemies.Remove(gameObject);
        }
        Destroy(gameObject, 0.2f);
    }

    public IEnumerator EnemyStaggered()
    {
        Debug.Log("Enemy is reeling! Riposte Ready!");
        gameObject.GetComponent<EnemyHealth>().isStaggered = true;
        yield return new WaitForSeconds(staggerDuration);
        gameObject.GetComponent<EnemyHealth>().isStaggered = false;
        gameObject.GetComponent<EnemyHealth>().currentHits--;
        gameObject.GetComponent<EnemyHealth>().canBeRiposted = false;
    }
}
