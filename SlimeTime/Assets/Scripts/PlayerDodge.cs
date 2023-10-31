using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDodge : MonoBehaviour
{
    public Dodge dodgeActive;
    [SerializeField] float dodgeDuration = 0.3f;
    [SerializeField] float riposteDelay = 0.2f;
    [SerializeField] float riposteRecovery = 0.5f;
    public bool riposteReady = false;
    public bool riposteActivated = false;
    public bool slimeShellReady = false;
    public bool slimeShellActive = false;
    public List<GameObject> riposteTargets;
    // Start is called before the first frame update
    void Start()
    {
        dodgeActive = Dodge.None;
        riposteActivated = false;
        riposteReady = false;
        slimeShellActive = false;
        slimeShellReady = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!riposteActivated)
        {
            DodgeInput();
        }

        if (riposteReady)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                StartCoroutine(RiposteActivate());
            }
        }

        if (slimeShellReady && !slimeShellActive)
        {
            if (Input.GetKeyDown(KeyCode.Y))
            {
                slimeShellActive = true;
            }
        }
    }

    private void DodgeInput()
    {
        if (dodgeActive == Dodge.None)
        {
            if (Input.GetKeyDown(KeyCode.RightShift))
            {
                StartCoroutine(DodgeTimer(Dodge.Horizontal));
            }
            else if (Input.GetKeyDown(KeyCode.Return))
            {
                StartCoroutine(DodgeTimer(Dodge.Vertical));
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine(DodgeTimer(Dodge.Reflect));
            }
        }
    }

    public enum Dodge
    {
        None,
        Horizontal,
        Vertical,
        Reflect
    }

    private IEnumerator DodgeTimer(Dodge dodge)
    {
        dodgeActive = dodge;
        yield return new WaitForSeconds(dodgeDuration);
        dodgeActive = Dodge.None;
    }

    private IEnumerator RiposteActivate()
    {
        if (!riposteActivated)
        {
            riposteActivated = true;
            yield return new WaitForSeconds(riposteDelay);
            GameObject riposteTarget = riposteTargets[0];
            Debug.Log("A killing blow!");
            riposteTargets.Remove(riposteTarget);
            riposteTarget.GetComponent<EnemyHealth>().Death();
            riposteReady = false;
            yield return new WaitForSeconds(riposteRecovery);
            riposteActivated = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "horizontalAttack")
        {
            if (dodgeActive == Dodge.Horizontal)
            {
                //ignore damage
                //add riposte to enemy health
                DamageEnemy(collision.gameObject);
                Debug.Log("horizontal attack riposted");
            }
            else
            {
                GameObject.Find("Game Manager").GetComponent<GameManager>().isDead = true;
                Debug.Log("You Died!");
                Destroy(gameObject, 0.2f);
            }
        }

        else if (collision.gameObject.tag == "verticalAttack")
        {
            if (dodgeActive == Dodge.Vertical)
            {
                //ignore damage
                //add riposte to enemy health
                DamageEnemy(collision.gameObject);
                Debug.Log("vertical attack riposted");
            }
            else
            {
                GameObject.Find("Game Manager").GetComponent<GameManager>().isDead = true;
                Debug.Log("You Died!");
                Destroy(gameObject, 0.2f);
            }
        }
        else if (collision.gameObject.tag == "reflectiveAttack")
        {
            if (dodgeActive == Dodge.Reflect)
            {
                Debug.Log("attack reflected");
            }
            else
            {
                GameObject.Find("Game Manager").GetComponent<GameManager>().isDead = true;
                Debug.Log("You Died!");
                Destroy(gameObject, 0.2f);
            }
        }
    }
    private void DamageEnemy(GameObject enemy)
    {
        if (enemy.GetComponentInParent<EnemyHealth>().currentHits < enemy.gameObject.GetComponentInParent<EnemyHealth>().maxHits)
        {
            enemy.gameObject.GetComponentInParent<EnemyHealth>().currentHits++;
        }
        else
        {
            enemy.gameObject.GetComponentInParent<EnemyHealth>().canBeRiposted = true;
            Debug.Log("Enemy can be riposted!");
        }
    }
}
