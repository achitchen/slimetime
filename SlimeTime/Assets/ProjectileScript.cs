using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Enemy")
        {
            StartCoroutine("DestroySelf");
        }
    }

    private IEnumerator DestroySelf()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
