using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDodge : MonoBehaviour
{
    public Dodge dodgeActive;
    [SerializeField] float dodgeDuration = 0.3f;
    [SerializeField] float riposteDelay = 0.1f;
    [SerializeField] float riposteRecovery = 0.5f;
    [SerializeField] float slimeIFrameDelay = 0.2f;
    [SerializeField] GameObject slimeShield;
    public int slimeShellCount = 1;
    public bool riposteReady = false;
    public bool riposteActivated = false;
    public bool slimeShellActive = false;
    // Start is called before the first frame update
    void Start()
    {
        dodgeActive = Dodge.None;
        riposteActivated = false;
        riposteReady = false;
        slimeShellActive = false;
        slimeShellCount = GetComponent<ReloadAtCheckpoint>().slimeShellCountInit;
    }

    // Update is called once per frame
    void Update()
    {
        if (!riposteActivated)
        {
            DodgeInput();
        }

        if (slimeShellCount > 0 && !slimeShellActive)
        {
            if (Input.GetKeyDown(KeyCode.Y))
            {
                slimeShield.SetActive(true);
                slimeShellActive = true;
                slimeShellCount--;
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

    public IEnumerator RiposteActivate()
    {
        if (!riposteActivated)
        {
            riposteActivated = true;
            yield return new WaitForSeconds(riposteDelay);
            Debug.Log("A killing blow!");
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
                //add riposte to enemy health
                DamageEnemy(collision.gameObject);
                Debug.Log("horizontal attack riposted");
            }
            else if (slimeShellActive)
            {
                StartCoroutine("DeactivateSlimeShield");
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
            else if (slimeShellActive)
            {
                StartCoroutine("DeactivateSlimeShield");
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
            else if (slimeShellActive)
            {
                StartCoroutine("DeactivateSlimeShield");
            }
            else
            {
                GameObject.Find("Game Manager").GetComponent<GameManager>().isDead = true;
                Debug.Log("You Died!");
                Destroy(gameObject, 0.2f);
            }
        }
        else if (collision.gameObject.tag == "RiposteArea")
        {
            riposteReady = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "RiposteArea")
        {
            riposteReady = false;
        }
    }
    private void DamageEnemy(GameObject enemy)
    {
        enemy.gameObject.GetComponentInParent<EnemyHealth>().currentHits++;
        if (enemy.GetComponentInParent<EnemyHealth>().currentHits == enemy.gameObject.GetComponentInParent<EnemyHealth>().maxHits)
        {
            enemy.gameObject.GetComponentInParent<EnemyHealth>().canBeRiposted = true;
            Debug.Log("Enemy can be riposted!");
        }
    }

    public IEnumerator DeactivateSlimeShield()
    {
        yield return new WaitForSeconds(slimeIFrameDelay);
        slimeShield.SetActive(false);
        slimeShellActive = false;

    }
}
