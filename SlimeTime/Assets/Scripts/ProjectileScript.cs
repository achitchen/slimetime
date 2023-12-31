using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    [SerializeField] GameObject reflectedProjectile;

    private void Start()
    {
        gameObject.name = "Projectile";
    }

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
                if (collision.GetComponent<PlayerDodge>().slimeShellActive)
                {
                    collision.GetComponent<PlayerDodge>().StartCoroutine("DeactivateSlimeShield");
                }
                else
                {
                    GameObject.Find("Game Manager").GetComponent<GameManager>().isDead = true;
                    Debug.Log("You Died!");
                    Destroy(collision.gameObject, 0.2f);
                }
             GetComponent<Collider2D>().enabled = false;
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
