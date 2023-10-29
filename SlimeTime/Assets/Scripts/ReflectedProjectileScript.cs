using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectedProjectileScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            StartCoroutine("DestroySelf");
        }
        else if (collision.gameObject.tag == "Enemy")
        {
            DamageEnemy(collision.gameObject);
            StartCoroutine("DestroySelf");
            //damage enemy
        }
    }
    private IEnumerator DestroySelf()
    {
        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);
    }

    private void DamageEnemy(GameObject enemy)
    {
        if (enemy.GetComponent<EnemyHealth>().currentHits < enemy.gameObject.GetComponent<EnemyHealth>().maxHits)
        {
            enemy.gameObject.GetComponent<EnemyHealth>().currentHits++;
        }
        else
        {
            enemy.gameObject.GetComponent<EnemyHealth>().canBeRiposted = true;
            enemy.gameObject.GetComponent<EnemyHealth>().StartCoroutine("EnemyStaggered");
        }
    }
}
