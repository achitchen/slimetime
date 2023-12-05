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
    [SerializeField] GameObject verticalAttack;
    [SerializeField] GameObject horizontalAttack;
    [SerializeField] GameObject spriteObject;
    [SerializeField] GameObject horizontalIcon;
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
                    animator.ResetTrigger("horizontalTrigger");
                    animator.ResetTrigger("hitTrigger");
                    animator.ResetTrigger("verticalTrigger");
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
                        animator.ResetTrigger("horizontalTrigger");
                        animator.ResetTrigger("hitTrigger");
                        animator.ResetTrigger("verticalTrigger");
                }
            }
            else if (!isStaggered && nearRadius >= targetDist)
            {
                MeleeAttack();
            }
        }
        if (GetComponent<EnemyHealth>().canBeRiposted)
        {
            animator.SetTrigger("hitTrigger");
            if (!isStaggered)
            {
                GetComponent<EnemyHealth>().StartCoroutine("EnemyStaggered");
                animator.ResetTrigger("idleTrigger");
                animator.ResetTrigger("horizontalTrigger");
                animator.ResetTrigger("verticalTrigger");
                animator.ResetTrigger("hitTrigger");
                animator.ResetTrigger("runTrigger");
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
                StartCoroutine(AttackWindup(AttackType.horizontal));
            }
            else if (attackIndex >= 51)
            {
                StartCoroutine(AttackWindup(AttackType.vertical));
            }
            canAttack = false;
        }
    }

    private IEnumerator AttackWindup(AttackType attackType)
    {
        Vector2 attackPos = targetPos;
        attackTelegraph.transform.position = attackPos;
        verticalAttack.transform.position = attackPos;
        horizontalAttack.transform.position = attackPos;
        if (attackType == AttackType.horizontal)
        {
            horizontalIcon.GetComponent<SpriteRenderer>().flipX = spriteObject.GetComponent<SpriteRenderer>().flipX;
            animator.SetTrigger("horizontalTrigger");
        }
        else if (attackType == AttackType.vertical)
        {
            animator.SetTrigger("verticalTrigger");
        }
        animator.ResetTrigger("runTrigger");
        animator.ResetTrigger("idleTrigger");
        animator.ResetTrigger("hitTrigger");
        isAttacking = true;
        moveDir = Vector3.zero;
        yield return new WaitForSeconds(attackDelay);
        yield return new WaitForSeconds(attackActiveTime);
        animator.ResetTrigger("horizontalTrigger");
        animator.ResetTrigger("verticalTrigger");
        animator.SetTrigger("idleTrigger");
        yield return new WaitForSeconds(attackRecovery);
        isAttacking = false;
        canAttack = true;
    }

    public void resetBools()
    {
        canAttack = true;
        isAttacking = false;
        animator.ResetTrigger("idleTrigger");
        animator.ResetTrigger("horizontalTrigger");
        animator.ResetTrigger("runTrigger");
        animator.ResetTrigger("hitTrigger");
        animator.ResetTrigger("verticalTrigger");
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
