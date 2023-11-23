using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CyclopsState : MonoBehaviour
{
    [SerializeField] float nearRadius = 1;
    [SerializeField] int farRadius = 5;
    [SerializeField] int speed = 2;
    [SerializeField] float targetDist;
    [SerializeField] float attackDelay = 1f;
    [SerializeField] float attackActive = 0.2f;
    [SerializeField] float doubleAttackPause = 0.4f;
    [SerializeField] float attackRecovery = 0.6f;
    [SerializeField] GameObject attackTelegraph;
    [SerializeField] GameObject attack;
    [SerializeField] GameObject spriteObject;
    private Animator animator;
    private bool canAttack = true;
    private bool isAttacking = false;
    private bool isStaggered;
    private Vector3 moveDir;
    private Vector3 targetPos;
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        if (player == null)
        {
            player = GameObject.Find("Player");
            Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        isStaggered = gameObject.GetComponent<EnemyHealth>().isStaggered;
        if (player != null && !player.GetComponent<PlayerDodge>().riposteActivated)
        {
            targetPos = player.transform.position;
            targetDist = Vector3.Distance(targetPos, transform.position);
            if (!isAttacking)
            {
                if ((targetPos - transform.position).normalized.x < 0)
                {
                    spriteObject.GetComponent<SpriteRenderer>().flipX = true;
                }
                else
                {
                    spriteObject.GetComponent<SpriteRenderer>().flipX = false;
                }
            }
            if (targetDist > farRadius)
            {
                animator.SetTrigger("idleTrigger");
                animator.ResetTrigger("runTrigger");
                animator.ResetTrigger("attackTrigger");
                animator.ResetTrigger("hitTrigger");
            }

            if (farRadius >= targetDist && targetDist > nearRadius && !isStaggered)
            {
                if (!isAttacking)
                {
                    moveDir = (targetPos - transform.position).normalized;
                    transform.Translate(moveDir * speed * Time.deltaTime);
                    animator.SetTrigger("runTrigger");
                    animator.ResetTrigger("idleTrigger");
                    animator.ResetTrigger("attackTrigger");
                    animator.ResetTrigger("hitTrigger");
                }
            }
            else if (targetDist <= nearRadius)
            {
                if (GetComponent<EnemyHealth>().canBeRiposted)
                {
                    animator.SetTrigger("hitTrigger");
                    if (!isStaggered)
                    {
                        GetComponent<EnemyHealth>().StartCoroutine("EnemyStaggered");
                        animator.ResetTrigger("runTrigger");
                        animator.ResetTrigger("idleTrigger");
                        animator.ResetTrigger("attackTrigger");
                    }
                }
                else if (!isStaggered)
                {
                    ChooseAttack();
                }
            }
        }
    }
    private void ChooseAttack()
    {
        if (canAttack)
        {
            int attackIndex = Random.Range(1, 100);
            if (attackIndex >= 25)
            {
                StartCoroutine("MeleeAttack");
            }
            else
            {
                StartCoroutine("DoubleAttack");
            }
            canAttack = false;
        }
    }

    private IEnumerator MeleeAttack()
    {
        isAttacking = true;
        animator.ResetTrigger("runTrigger");
        animator.ResetTrigger("idleTrigger");
        animator.SetTrigger("attackTrigger");
        animator.ResetTrigger("hitTrigger");
        Vector2 attackPos = targetPos;
        attackTelegraph.transform.position = attackPos;
        moveDir = Vector3.zero;
        yield return new WaitForSeconds(attackDelay);
        attack.transform.position = attackPos;
        yield return new WaitForSeconds(attackActive);
        animator.ResetTrigger("attackTrigger");
        animator.SetTrigger("idleTrigger");
        yield return new WaitForSeconds(attackRecovery);
        isAttacking = false;
        canAttack = true;
    }

    private IEnumerator DoubleAttack()
    {
        animator.ResetTrigger("runTrigger");
        animator.ResetTrigger("idleTrigger");
        animator.SetTrigger("attackTrigger");
        animator.ResetTrigger("hitTrigger");
        isAttacking = true;
        Vector2 attackPos = targetPos;
        attackTelegraph.transform.position = attackPos;
        moveDir = Vector3.zero;
        yield return new WaitForSeconds(attackDelay);
        attackTelegraph.SetActive(false);
        attack.SetActive(true);
        attack.transform.position = attackPos;
        yield return new WaitForSeconds(attackActive);
        animator.ResetTrigger("attackTrigger");
        attack.SetActive(false);
        yield return new WaitForSeconds(doubleAttackPause);
        animator.SetTrigger("attackTrigger");
        attack.SetActive(true);
        yield return new WaitForSeconds(attackActive);
        animator.ResetTrigger("attackTrigger");
        animator.SetTrigger("idleTrigger");
        attack.SetActive(false);
        yield return new WaitForSeconds(attackRecovery);
        isAttacking = false;
        canAttack = true;
    }


    public void resetBools()
    {
        canAttack = true;
        isAttacking = false;
        isStaggered = false;
        animator.ResetTrigger("idleTrigger");
        animator.ResetTrigger("attackTrigger");
        animator.ResetTrigger("runTrigger");
        animator.ResetTrigger("hitTrigger");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Wall")
            if (!isAttacking)
            {
                Vector3 collisionDir = -(collision.gameObject.transform.position - transform.position).normalized;
                transform.Translate(collisionDir * 0.1f);
            }
    }
}
