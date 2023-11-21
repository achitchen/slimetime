using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponImp : MonoBehaviour
{
    [SerializeField] int nearRadius = 1;
    [SerializeField] int farRadius = 5;
    [SerializeField] int speed = 2;
    [SerializeField] float targetDist;
    [SerializeField] float attackDelay = 1f;
    [SerializeField] float attackActiveTime = 0.5f;
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

    void Start()
    {
        if (player == null)
        {
            player = GameObject.Find("Player");
            Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }
        animator = GetComponent<Animator>();
        resetBools();
    }


    void Update()
    {
        isStaggered = gameObject.GetComponent<EnemyHealth>().isStaggered;
        if (player != null && !player.GetComponent<PlayerDodge>().riposteActivated)
        {
            targetPos = player.transform.position;
            targetDist = Vector3.Distance(targetPos, transform.position);
            if ((targetPos - transform.position).normalized.x < 0)
            {
                spriteObject.GetComponent<SpriteRenderer>().flipX = true;
            }
            else
            {
                spriteObject.GetComponent<SpriteRenderer>().flipX = false;
            }
            if (targetDist > farRadius && !isStaggered)
            {
                if (!isAttacking)
                {
                    moveDir = (targetPos - transform.position).normalized;
                    transform.Translate(moveDir * speed * Time.deltaTime);
                    animator.SetTrigger("runTrigger");
                    animator.ResetTrigger("idleTrigger");
                    animator.ResetTrigger("attackTrigger");
                    animator.ResetTrigger("windupTrigger");
                    animator.ResetTrigger("hitTrigger");
                }
            }
            else if (farRadius >= targetDist && targetDist > nearRadius && !isStaggered)
            {
                if (!isAttacking)
                {
                    moveDir = (targetPos - transform.position).normalized;
                    transform.Translate(moveDir * speed * Time.deltaTime);
                    animator.SetTrigger("runTrigger");
                    animator.ResetTrigger("idleTrigger");
                    animator.ResetTrigger("attackTrigger");
                    animator.ResetTrigger("windupTrigger");
                    animator.ResetTrigger("hitTrigger");
                }
            }
            else if (GetComponent<EnemyHealth>().canBeRiposted)
            {
                animator.SetTrigger("hitTrigger");
                if (!isStaggered)
                {
                    GetComponent<EnemyHealth>().StartCoroutine("EnemyStaggered");
                    animator.ResetTrigger("runTrigger");
                    animator.ResetTrigger("idleTrigger");
                    animator.ResetTrigger("attackTrigger");
                    animator.ResetTrigger("windupTrigger");
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
        animator.SetTrigger("windupTrigger");
        animator.ResetTrigger("runTrigger");
        animator.ResetTrigger("idleTrigger");
        animator.ResetTrigger("attackTrigger");
        animator.ResetTrigger("hitTrigger");
        isAttacking = true;
        Vector2 attackPos = targetPos;
        attackTelegraph.SetActive(true);
        attackTelegraph.transform.position = attackPos;
        moveDir = Vector3.zero;
        yield return new WaitForSeconds(attackDelay);
        animator.ResetTrigger("windupTrigger");
        animator.SetTrigger("attackTrigger");
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
        animator.SetTrigger("idleTrigger");
        animator.ResetTrigger("attackTrigger");
        animator.ResetTrigger("windupTrigger");
        animator.ResetTrigger("runTrigger");
        animator.ResetTrigger("hitTrigger");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Wall")
        {
            Vector3 collisionDir = -(collision.gameObject.transform.position - transform.position).normalized;
            transform.Translate(collisionDir * 0.1f);
        }
    }
}
