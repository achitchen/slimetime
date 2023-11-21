using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightState : MonoBehaviour
{
    [SerializeField] float nearRadius = 1.5f;
    [SerializeField] int speed = 2;
    [SerializeField] float targetDist;
    [SerializeField] float attackDelay = 1f;
    [SerializeField] float attackActive = 0.5f;
    [SerializeField] float jumpDelay = 1f;
    [SerializeField] float jumpAttackActive = 0.5f;
    [SerializeField] float doubleAttackPause = 0.2f;
    [SerializeField] float attackRecovery = 0.6f;
    [SerializeField] GameObject attackTelegraph;
    [SerializeField] GameObject jumpTelegraph;
    [SerializeField] GameObject attack;
    [SerializeField] GameObject jumpAttack;
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
    }

    // Update is called once per frame
    void Update()
    {
        isStaggered = gameObject.GetComponent<EnemyHealth>().isStaggered;
        if (player != null && !player.GetComponent<PlayerDodge>().riposteActivated)
        {
            targetPos = player.transform.position;
            targetDist = Vector3.Distance(targetPos, transform.position);

            if (targetDist > nearRadius && !isStaggered)
            {
                if (!isAttacking)
                {
                    moveDir = (targetPos - transform.position).normalized;
                    transform.Translate(moveDir * speed * Time.deltaTime);
                }
            }
            else if (GetComponent<EnemyHealth>().canBeRiposted)
            {
                if (!isStaggered)
                {
                    GetComponent<EnemyHealth>().StartCoroutine("EnemyStaggered");
                }
            }
            else if (!isStaggered)
            {
                ChooseAttack();
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
            else if (attackIndex >= 25)
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

    private IEnumerator JumpAttack()
    {
        isAttacking = true;
        Vector2 attackPos = targetPos;
        jumpTelegraph.SetActive(true);
        jumpTelegraph.transform.position = transform.position;
        moveDir = Vector3.zero;
        yield return new WaitForSeconds(jumpDelay);
        jumpTelegraph.SetActive(false);
        jumpAttack.SetActive(true);
        yield return new WaitForSeconds(jumpAttackActive);
        jumpAttack.SetActive(false);
        isAttacking = false;
        canAttack = true;

    }

    private IEnumerator MeleeAttack()
    {
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
        yield return new WaitForSeconds(attackActive);
        attack.SetActive(false);
        moveDir = (targetPos - transform.position).normalized;
        transform.Translate(moveDir * speed * Time.deltaTime);
        yield return new WaitForSeconds(attackRecovery);
        isAttacking = false;
        canAttack = true;
    }

    private IEnumerator DoubleAttack()
    {
        isAttacking = true;
        Vector2 attackPos = targetPos;
        attackTelegraph.SetActive(true);
        attackTelegraph.transform.position = attackPos;
        moveDir = Vector3.zero;
        yield return new WaitForSeconds(attackDelay);
        attackTelegraph.SetActive(false);
        attack.SetActive(true);
        attack.transform.position = attackPos;
        yield return new WaitForSeconds(attackActive);
        attack.SetActive(false);
        yield return new WaitForSeconds(doubleAttackPause);
        attack.SetActive(true);
        yield return new WaitForSeconds(attackActive);
        attack.SetActive(false);
        moveDir = (targetPos - transform.position).normalized;
        transform.Translate(moveDir * speed * Time.deltaTime);
        yield return new WaitForSeconds(attackRecovery);
        isAttacking = false;
        canAttack = true;
    }
}
