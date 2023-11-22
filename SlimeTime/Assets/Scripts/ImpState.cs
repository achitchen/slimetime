using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpState : MonoBehaviour
{
    [SerializeField] int nearRadius = 1;
    [SerializeField] int midRadius = 3;
    [SerializeField] int speed = 2;
    [SerializeField] float targetDist;
    [SerializeField] float attackDelay = 1f;
    [SerializeField] float attackActiveTime = 0.5f;
    [SerializeField] float attackRecovery = 1.5f;
    [SerializeField] GameObject attackTelegraph;
    [SerializeField] GameObject attack;
    [SerializeField] GameObject spriteObject;
    private bool canAttack = true;
    private bool isAttacking = false;
    private bool isStaggered;
    private Animator animator;
    private Vector3 moveDir;
    private Vector3 targetPos;
    private GameObject player;

    void Start()
    {
        if (player == null)
        {
            player = GameObject.Find("Player");
            Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }
        animator = GetComponent<Animator>();
    }


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
            if (targetDist > midRadius && !isStaggered)
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
            else if (midRadius >= targetDist && targetDist > nearRadius && !isStaggered)
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
            else if (GetComponent<EnemyHealth>().canBeRiposted)
            {
                animator.ResetTrigger("runTrigger");
                animator.SetTrigger("hitTrigger");
                if (!isStaggered)
                {
                    GetComponent<EnemyHealth>().StartCoroutine("EnemyStaggered");
                    animator.ResetTrigger("idleTrigger");
                    animator.ResetTrigger("attackTrigger");
                }
            }
            else if (!isStaggered && nearRadius >= targetDist)
            {
                MeleeAttack();
            }
        }
    }

    private void MeleeAttack()
    {
        if (canAttack)
        {
            StartCoroutine("AttackWindup");
            canAttack = false;
        }
    }

    private IEnumerator AttackWindup()
    {
        animator.SetTrigger("attackTrigger");
        animator.ResetTrigger("runTrigger");
        animator.ResetTrigger("idleTrigger");
        animator.ResetTrigger("hitTrigger");
        isAttacking = true;
        Vector2 attackPos = targetPos;
        attackTelegraph.SetActive(true);
        attackTelegraph.transform.position = attackPos;
        moveDir = Vector3.zero;
        yield return new WaitForSeconds(attackDelay);
        attackTelegraph.SetActive(false);
        attack.SetActive(true);
        attack.transform.position = attackPos;
        Debug.Log("Bite!");
        yield return new WaitForSeconds(attackActiveTime);
        attack.SetActive(false);
        //moveDir = (player.transform.position - transform.position).normalized;
        animator.ResetTrigger("attackTrigger");
        animator.SetTrigger("idleTrigger");
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
        {
            if (!isAttacking)
            {
                Vector3 collisionDir = -(collision.gameObject.transform.position - transform.position).normalized;
                transform.Translate(collisionDir * 0.1f);
            }
        }
    }
}
