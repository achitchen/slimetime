using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostState : MonoBehaviour
{
    [SerializeField] int nearRadius = 1;
    [SerializeField] int midRadius = 3;
    [SerializeField] int farRadius = 5;
    [SerializeField] int speed = 2;
    [SerializeField] float targetDist;
    [SerializeField] float attackDelay = 1f;
    [SerializeField] float attackRecovery = 0.6f;
    [SerializeField] float teleportOffset = 1f;
    [SerializeField] float teleportDelay = 1f;
    [SerializeField] float teleportRecharge = 3f;
    [SerializeField] float teleportTelegraphTime = 0.5f;
    [SerializeField] GameObject spriteObject;
    [SerializeField] GameObject attackTelegraph;
    [SerializeField] GameObject horizontalAttack;
    [SerializeField] GameObject reflectiveAttack;
    [SerializeField] GameObject horizontalIcon;
    private bool canAttack = true;
    private bool isAttacking = false;
    private bool isStaggered;
    private bool isTeleporting = false;
    private bool canTeleport = true;
    private Animator animator;
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
        resetBools();
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
                if (canTeleport)
                {
                    StartCoroutine("Teleport");
                }
            }

            else if (farRadius >= targetDist && targetDist > midRadius && !isStaggered && !isTeleporting)
            {
                if (!isAttacking)
                {
                    moveDir = -(targetPos - transform.position).normalized;
                    transform.Translate(moveDir * speed * Time.deltaTime);
                    animator.SetTrigger("runTrigger");
                    animator.ResetTrigger("idleTrigger");
                    animator.ResetTrigger("horizontalTrigger");
                    animator.ResetTrigger("reflectiveTrigger");
                    animator.ResetTrigger("hitTrigger");
                    animator.ResetTrigger("teleportTrigger");
                }
            }
            else if (midRadius >= targetDist && targetDist > nearRadius && !isStaggered && !isTeleporting)
            {
                moveDir = (targetPos - transform.position).normalized;
                transform.Translate(moveDir * speed * Time.deltaTime);
                animator.SetTrigger("runTrigger");
                animator.ResetTrigger("idleTrigger");
                animator.ResetTrigger("horizontalTrigger");
                animator.ResetTrigger("reflectiveTrigger");
                animator.ResetTrigger("hitTrigger");
                animator.ResetTrigger("teleportTrigger");
            }
            else if (GetComponent<EnemyHealth>().canBeRiposted)
            {
                animator.ResetTrigger("runTrigger");
                animator.SetTrigger("hitTrigger");
                if (!isStaggered)
                {
                    GetComponent<EnemyHealth>().StartCoroutine("EnemyStaggered");
                    animator.ResetTrigger("idleTrigger");
                    animator.ResetTrigger("horizontalTrigger");
                    animator.ResetTrigger("reflectiveTrigger");
                    animator.ResetTrigger("teleportTrigger");
                }
            }
            else if (targetDist <= nearRadius && !isStaggered)
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
            if (attackIndex < 70)
            {
                StartCoroutine(AttackWindup(targetPos, AttackType.horizontal));
            }
            else if (attackIndex >= 70)
            {
                StartCoroutine(AttackWindup(targetPos, AttackType.reflect));
            }
            canAttack = false;
        }
    }

    private IEnumerator AttackWindup(Vector2 attackPos, AttackType attackType)
    {
        if (attackType == AttackType.horizontal)
        {
            horizontalIcon.GetComponent<SpriteRenderer>().flipX = spriteObject.GetComponent<SpriteRenderer>().flipX;
            animator.SetTrigger("horizontalTrigger");
            horizontalAttack.transform.position = attackPos;
        }
        else if (attackType == AttackType.reflect)
        {
            animator.SetTrigger("reflectiveTrigger");
            reflectiveAttack.transform.position = attackPos;
        }
        animator.ResetTrigger("runTrigger");
        animator.ResetTrigger("idleTrigger");
        animator.ResetTrigger("hitTrigger");
        animator.ResetTrigger("teleportTrigger");
        isAttacking = true;
        attackTelegraph.transform.position = attackPos;
        moveDir = Vector3.zero;
        yield return new WaitForSeconds(attackDelay);
        animator.ResetTrigger("horizontalTrigger");
        animator.ResetTrigger("reflectiveTrigger");
        animator.SetTrigger("idleTrigger");
        moveDir = (targetPos - transform.position).normalized;
        transform.Translate(moveDir * speed * Time.deltaTime);
        yield return new WaitForSeconds(attackRecovery);
        isAttacking = false;
        canAttack = true;
    }


    public void resetBools()
    {
        canAttack = true;
        isAttacking = false;
        isStaggered = false;
        canTeleport = true;
        isTeleporting = false;
        animator.ResetTrigger("runTrigger");
        animator.ResetTrigger("idleTrigger");
        animator.ResetTrigger("horizontalTrigger");
        animator.ResetTrigger("hitTrigger");
        animator.ResetTrigger("teleportTrigger");
        animator.ResetTrigger("reflectiveTrigger");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Vector3 collisionDir = -(collision.gameObject.transform.position - transform.position).normalized;
            transform.Translate(collisionDir * 0.1f);
        }
        else if (collision.gameObject.tag == "Wall")
        {
            StartCoroutine("Teleport");
        }
    }

    private IEnumerator Teleport()
    {
        canTeleport = false;
        isTeleporting = true;
        animator.SetTrigger("teleportTrigger");
        yield return new WaitForSeconds(teleportDelay);
        Vector3 teleportPos = player.transform.position + (player.transform.right * teleportOffset);
        transform.position = teleportPos;
        yield return new WaitForSeconds(teleportTelegraphTime);
        MeleeAttack();
        yield return new WaitForSeconds(teleportRecharge);
        canTeleport = true;
        isTeleporting = false;

    }
    private enum AttackType
    {
        horizontal,
        vertical,
        reflect
    }
}
