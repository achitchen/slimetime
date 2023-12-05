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
    [SerializeField] GameObject riposteRadius;
    public GameObject assignedRoom;
    private GameObject player;

    void Start()
    {
        currentHits = 0;
        canBeRiposted = false;
        player = GameObject.Find("Player");
    }

    private void Update()
    {
        if (player != null && player.activeSelf == true)
        {
            if (player.GetComponent<PlayerDodge>().riposteReady = true && canBeRiposted)
            {
                if (Input.GetKeyDown(KeyCode.R))
                {
                    player.GetComponent<PlayerDodge>().RiposteTrigger(gameObject.transform);
                    StopCoroutine("EnemyStaggered");
                    isStaggered = true;
                    Death();
                }
            }
        }
    }

    public void Death()
    {
        if (gameObject.GetComponent<DoorController>().assignedDoor != null)
        {
            gameObject.GetComponent<DoorController>().assignedDoor.GetComponent<CombatDoor>().roomEnemies.Remove(gameObject);
        }
        if (assignedRoom != null)
        {
            assignedRoom.GetComponent<EnemyManager>().enemies.Remove(gameObject);
        }
        Destroy(gameObject, 0.3f);
    }

    public IEnumerator EnemyStaggered()
    {
        if (gameObject.GetComponent<EnemySounds>() != null)
        {
            GetComponent<EnemySounds>().enemySoundSource.PlayOneShot(GetComponent<EnemySounds>().enemyStaggeredSound);
        }
        
        riposteRadius.SetActive(true);
        Debug.Log("Enemy is reeling! Riposte Ready!");
        canBeRiposted = true;
        isStaggered = true;
        yield return new WaitForSeconds(staggerDuration);
        riposteRadius.SetActive(false);
        isStaggered = false;
        if (gameObject.GetComponent<CyclopsState>()!= null)
        {
            currentHits -= 2;
        }
        else if (gameObject.GetComponent<KnightState>() != null)
        {
            currentHits -= 3;
        }
        else
        {
            currentHits--;
        }
        canBeRiposted = false;
    }
}
