using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightState : MonoBehaviour
{
    public int KnightIdentifier;
    [SerializeField] float nearRadius = 1.5f;
    [SerializeField] int speed = 2;
    [SerializeField] float targetDist;
    [SerializeField] float attackDelay = 1f;
    [SerializeField] float attackActive = 0.5f;
    [SerializeField] float jumpDelay = 1f;
    [SerializeField] float jumpAttackActive = 0.5f;
    [SerializeField] float doubleAttackPause = 0.2f;
    [SerializeField] float attackRecovery = 0.6f;
    [SerializeField] GameObject spriteObject;
    [SerializeField] GameObject attackTelegraph;
    [SerializeField] GameObject jumpTelegraph;
    [SerializeField] GameObject verticalAttack;
    [SerializeField] GameObject horizontalAttack;
    [SerializeField] GameObject jumpAttack;
    [SerializeField] GameObject horizontalIcon;
    private bool canAttack = true;
    private bool isAttacking = false;
    private bool isStaggered;
    private Vector3 moveDir;
    private Vector3 targetPos;
    private GameObject player;
    private Animator animator;
    private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        if (player == null)
        {
            player = GameObject.Find("Player");
            Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }
        if (gameManager == null)
        {
            gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        }
        InitiateKnight();
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
            if ((targetPos - transform.position).normalized.x < 0)
            {
                spriteObject.GetComponent<SpriteRenderer>().flipX = true;
            }
            else
            {
                spriteObject.GetComponent<SpriteRenderer>().flipX = false;
            }
            if (targetDist > nearRadius && !isStaggered)
            {
                if (!isAttacking)
                {
                    moveDir = (targetPos - transform.position).normalized;
                    transform.Translate(moveDir * speed * Time.deltaTime);
                    animator.SetTrigger("runTrigger");
                    animator.ResetTrigger("idleTrigger");
                    animator.ResetTrigger("attack0Trigger");
                    animator.ResetTrigger("windup0Trigger");
                    animator.ResetTrigger("attack1Trigger");
                    animator.ResetTrigger("windup1Trigger");
                    animator.ResetTrigger("attack2Trigger");
                    animator.ResetTrigger("windup2Trigger");
                    animator.ResetTrigger("hitTrigger");
                }
            }
            else if (!isStaggered)
            {
                ChooseAttack();
            }
        }
        if (GetComponent<EnemyHealth>().canBeRiposted)
        {
            animator.SetTrigger("hitTrigger");
            if (!isStaggered)
            {
                GetComponent<EnemyHealth>().StartCoroutine("EnemyStaggered");
                animator.ResetTrigger("runTrigger");
                animator.ResetTrigger("idleTrigger");
                animator.ResetTrigger("attack0Trigger");
                animator.ResetTrigger("windup0Trigger");
                animator.ResetTrigger("attack1Trigger");
                animator.ResetTrigger("windup1Trigger");
                animator.ResetTrigger("attack2Trigger");
                animator.ResetTrigger("windup2Trigger");
            }
        }
    }

    private void ChooseAttack()
    {
        if (canAttack)
        {
            int attackIndex = Random.Range(1, 100);
            if (attackIndex >= 75)
            {
                StartCoroutine("JumpAttack");
            }
            else if (attackIndex >= 30)
            {
                StartCoroutine("MeleeAttack");
            }
            else if (attackIndex < 30)
            {
                StartCoroutine("DoubleAttack");
            }
            canAttack = false;
        }
    }

    private IEnumerator JumpAttack()
    {
        animator.SetTrigger("windup2Trigger");
        animator.ResetTrigger("runTrigger");
        animator.ResetTrigger("idleTrigger");
        animator.ResetTrigger("attack2Trigger");
        animator.ResetTrigger("hitTrigger");
        animator.ResetTrigger("attack0Trigger");
        animator.ResetTrigger("windup0Trigger");
        animator.ResetTrigger("attack1Trigger");
        animator.ResetTrigger("windup1Trigger");
        isAttacking = true;
        jumpTelegraph.transform.position = transform.position;
        moveDir = Vector3.zero;
        yield return new WaitForSeconds(jumpDelay);
        animator.SetTrigger("attack2Trigger");
        animator.ResetTrigger("windup2Trigger");
        yield return new WaitForSeconds(jumpAttackActive);
        animator.ResetTrigger("attack2Trigger");
        animator.SetTrigger("idleTrigger");
        yield return new WaitForSeconds(attackRecovery);
        isAttacking = false;
        canAttack = true;

    }

    private IEnumerator MeleeAttack()
    {
        animator.SetTrigger("windup0Trigger");
        animator.ResetTrigger("runTrigger");
        animator.ResetTrigger("idleTrigger");
        animator.ResetTrigger("attack0Trigger");
        animator.ResetTrigger("hitTrigger");
        animator.ResetTrigger("attack2Trigger");
        animator.ResetTrigger("windup2Trigger");
        animator.ResetTrigger("attack1Trigger");
        animator.ResetTrigger("windup1Trigger");
        isAttacking = true;
        Vector2 attackPos = targetPos;
        attackTelegraph.transform.position = attackPos;
        moveDir = Vector3.zero;
        yield return new WaitForSeconds(attackDelay);
        animator.SetTrigger("attack0Trigger");
        animator.ResetTrigger("windup0Trigger");
        verticalAttack.transform.position = attackPos;
        Debug.Log("Bite!");
        yield return new WaitForSeconds(attackActive);
        animator.SetTrigger("idleTrigger");
        animator.ResetTrigger("attack0Trigger");
        yield return new WaitForSeconds(attackRecovery);
        isAttacking = false;
        canAttack = true;
    }

    private IEnumerator DoubleAttack()
    {
        horizontalIcon.GetComponent<SpriteRenderer>().flipX = spriteObject.GetComponent<SpriteRenderer>().flipX;
        animator.SetTrigger("windup1Trigger");
        animator.ResetTrigger("runTrigger");
        animator.ResetTrigger("idleTrigger");
        animator.ResetTrigger("attack1Trigger");
        animator.ResetTrigger("hitTrigger");
        animator.ResetTrigger("attack2Trigger");
        animator.ResetTrigger("windup2Trigger");
        animator.ResetTrigger("attack0Trigger");
        animator.ResetTrigger("windup0Trigger");
        isAttacking = true;
        Vector2 attackPos = targetPos;
        attackTelegraph.transform.position = attackPos;
        moveDir = Vector3.zero;
        yield return new WaitForSeconds(attackDelay);
        animator.SetTrigger("attack1Trigger");
        animator.ResetTrigger("windup1Trigger");
        verticalAttack.transform.position = attackPos;
        horizontalAttack.transform.position = attackPos;
        yield return new WaitForSeconds(doubleAttackPause);
        animator.SetTrigger("idleTrigger");
        animator.ResetTrigger("attack1Trigger");
        yield return new WaitForSeconds(attackRecovery);
        isAttacking = false;
        canAttack = true;
    }

    private void InitiateKnight()
    {
        switch (KnightIdentifier)
        {
            case 0:
                if (gameManager.coreDestroyed1)
                {
                    if (gameObject.GetComponent<DoorController>().assignedDoor != null)
                    {
                        gameObject.GetComponent<DoorController>().assignedDoor.GetComponent<CombatDoor>().roomEnemies.Remove(gameObject);
                    }
                    if (gameObject.GetComponent<EnemyHealth>().assignedRoom != null)
                    {
                        gameObject.GetComponent<EnemyHealth>().assignedRoom.GetComponent<EnemyManager>().enemies.Remove(gameObject);
                    }
                    Destroy(gameObject);
                }
                break;
            case 1:
                if (gameManager.coreDestroyed2)
                {
                    if (gameObject.GetComponent<DoorController>().assignedDoor != null)
                    {
                        gameObject.GetComponent<DoorController>().assignedDoor.GetComponent<CombatDoor>().roomEnemies.Remove(gameObject);
                    }
                    if (gameObject.GetComponent<EnemyHealth>().assignedRoom != null)
                    {
                        gameObject.GetComponent<EnemyHealth>().assignedRoom.GetComponent<EnemyManager>().enemies.Remove(gameObject);
                    }
                    Destroy(gameObject);
                }
                break;
            case 2:
                if (gameManager.coreDestroyed3)
                {
                    if (gameObject.GetComponent<DoorController>().assignedDoor != null)
                    {
                        gameObject.GetComponent<DoorController>().assignedDoor.GetComponent<CombatDoor>().roomEnemies.Remove(gameObject);
                    }
                    if (gameObject.GetComponent<EnemyHealth>().assignedRoom != null)
                    {
                        gameObject.GetComponent<EnemyHealth>().assignedRoom.GetComponent<EnemyManager>().enemies.Remove(gameObject);
                    }
                    Destroy(gameObject);
                }
                break;
            case 3:
                if (gameManager.coreDestroyed3)
                {
                    if (gameObject.GetComponent<DoorController>().assignedDoor != null)
                    {
                        gameObject.GetComponent<DoorController>().assignedDoor.GetComponent<CombatDoor>().roomEnemies.Remove(gameObject);
                    }
                    if (gameObject.GetComponent<EnemyHealth>().assignedRoom != null)
                    {
                        gameObject.GetComponent<EnemyHealth>().assignedRoom.GetComponent<EnemyManager>().enemies.Remove(gameObject);
                    }
                    Destroy(gameObject);
                }
                break;
        }
    }
}
