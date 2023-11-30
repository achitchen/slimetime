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
    [SerializeField] GameObject verticalAttack;
    [SerializeField] GameObject reflectiveAttack;
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
            if (targetDist > farRadius && !isStaggered)
            {
                if (!isAttacking)
                {
                    moveDir = (targetPos - transform.position).normalized;
                    transform.Translate(moveDir * speed * Time.deltaTime);
                    animator.SetTrigger("runTrigger");
                    animator.ResetTrigger("idleTrigger");
                    animator.ResetTrigger("attackTrigger");
                    animator.ResetTrigger("hitTrigger");
                    animator.ResetTrigger("reflectiveTrigger");
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
                    animator.ResetTrigger("hitTrigger");
                    animator.ResetTrigger("reflectiveTrigger");
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
                    animator.ResetTrigger("hitTrigger");
                    animator.ResetTrigger("reflectiveTrigger");
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
            int attackIndex = Random.Range(1, 100);
            if (attackIndex < 51)
            {
                StartCoroutine(AttackWindup(AttackType.vertical));
            }
            else if (attackIndex >= 51)
            {
                StartCoroutine(AttackWindup(AttackType.reflect));
            }
            canAttack = false;
        }
    }

    private IEnumerator AttackWindup(AttackType attackType)
    {
        if (attackType == AttackType.vertical)
        {
            animator.SetTrigger("attackTrigger");
        }
        else if (attackType == AttackType.reflect)
        {
            animator.SetTrigger("reflectiveTrigger");
        }
        animator.ResetTrigger("runTrigger");
        animator.ResetTrigger("idleTrigger");
        animator.ResetTrigger("hitTrigger");
        isAttacking = true;
        Vector2 attackPos = targetPos;
        verticalAttack.transform.position = attackPos;
        reflectiveAttack.transform.position = attackPos;
        moveDir = Vector3.zero;
        yield return new WaitForSeconds(attackDelay);
        attack.transform.position = attackPos;
        yield return new WaitForSeconds(attackActiveTime);
        animator.ResetTrigger("attackTrigger");
        animator.ResetTrigger("reflectiveTrigger");
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
        animator.ResetTrigger("reflectiveTrigger");
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
    private enum AttackType
    {
        horizontal,
        vertical,
        reflect
    }
}
