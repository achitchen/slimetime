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
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        dodgeActive = Dodge.None;
        riposteActivated = false;
        riposteReady = false;
        slimeShellActive = false;
        slimeShellCount = GetComponent<ReloadAtCheckpoint>().slimeShellCountInit;
        animator = GetComponent<Animator>();
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
                animator.SetTrigger("splitTrigger"); 
                animator.ResetTrigger("idleTrigger");
                animator.ResetTrigger("runTrigger");
                animator.ResetTrigger("duckTrigger");
                animator.ResetTrigger("spikeTrigger");
                animator.ResetTrigger("reflectTrigger");
                StartCoroutine(DodgeTimer(Dodge.Horizontal));
            }
            else if (Input.GetKeyDown(KeyCode.Return))
            {
                animator.SetTrigger("duckTrigger");
                animator.ResetTrigger("idleTrigger");
                animator.ResetTrigger("runTrigger");
                animator.ResetTrigger("splitTrigger");
                animator.ResetTrigger("spikeTrigger");
                animator.ResetTrigger("reflectTrigger");
                StartCoroutine(DodgeTimer(Dodge.Vertical));
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                animator.SetTrigger("reflectTrigger");
                animator.ResetTrigger("idleTrigger");
                animator.ResetTrigger("runTrigger");
                animator.ResetTrigger("duckTrigger");
                animator.ResetTrigger("spikeTrigger");
                animator.ResetTrigger("splitTrigger");
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
        if (GetComponent<PlayerMovement>().isMoving)
        {
            animator.SetTrigger("runTrigger");
            animator.ResetTrigger("spikeTrigger");
            animator.ResetTrigger("idleTrigger");
            animator.ResetTrigger("duckTrigger");
            animator.ResetTrigger("splitTrigger");
            animator.ResetTrigger("reflectTrigger");
        }
        else
        {
            animator.SetTrigger("idleTrigger");
            animator.ResetTrigger("spikeTrigger");
            animator.ResetTrigger("runTrigger");
            animator.ResetTrigger("duckTrigger");
            animator.ResetTrigger("splitTrigger");
            animator.ResetTrigger("reflectTrigger");
        }
    }
    public void RiposteTrigger(Transform enemyPos)
    {
        StartCoroutine(RiposteActivate(enemyPos.transform));
        animator.SetTrigger("spikeTrigger");
        animator.ResetTrigger("idleTrigger");
        animator.ResetTrigger("runTrigger");
        animator.ResetTrigger("duckTrigger");
        animator.ResetTrigger("splitTrigger");
        animator.ResetTrigger("reflectTrigger");
    }

    public IEnumerator RiposteActivate(Transform enemyPos)
    {
        if (!riposteActivated)
        {
            riposteActivated = true;
            transform.position = enemyPos.position;
            yield return new WaitForSeconds(riposteDelay);
            riposteReady = false;
            yield return new WaitForSeconds(riposteRecovery);
            riposteActivated = false;
            animator.ResetTrigger("spikeTrigger");
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
                PlayerDeath();
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
                PlayerDeath();
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
                PlayerDeath();
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

    private void PlayerDeath()
    {
        GameObject.Find("Game Manager").GetComponent<GameManager>().isDead = true;
        GameObject.Find("Game Manager").GetComponent<GameManager>().Invoke("CanReload", 0.5f);
        Debug.Log("You Died!");
        Destroy(gameObject, 0.2f);
    }
}
