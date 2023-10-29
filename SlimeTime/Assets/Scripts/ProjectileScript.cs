using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    [SerializeField] GameObject reflectedProjectile;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            StartCoroutine("DestroySelf");
        }
        else if (collision.gameObject.tag == "Player")
        {
            if (collision.GetComponent<PlayerDodge>().dodgeActive != PlayerDodge.Dodge.Reflect)
            {
                StartCoroutine("DestroySelf");
                //kill player
            }
            else
            {
                Vector3 moveDir = -(GetComponent<Rigidbody2D>().velocity);
                GameObject reflectedClone = Instantiate(reflectedProjectile, transform.position, transform.rotation);
                reflectedClone.GetComponent<Rigidbody2D>().AddForce(moveDir * 2, ForceMode2D.Impulse);
                StartCoroutine("DestroySelf");
            }
        }
    }

    private IEnumerator DestroySelf()
    {
        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);
    }
}
