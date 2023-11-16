using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpState : MonoBehaviour
{
    [SerializeField] int nearRadius = 1;
    [SerializeField] int farRadius = 5;
    [SerializeField] int speed = 2;
    [SerializeField] float targetDist;
    [SerializeField] float attackDelay = 1f;
    [SerializeField] float attackActive = 0.5f;
    [SerializeField] float staggerDuration = 2.5f;
    [SerializeField] GameObject attackTelegraph;
    [SerializeField] GameObject attack;
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
        }
    }


    void Update()
    {
        isStaggered = gameObject.GetComponent<EnemyHealth>().isStaggered;
        if (player != null && !player.GetComponent<PlayerDodge>().riposteActivated)
        {
            targetPos = player.transform.position;
            targetDist = Vector3.Distance(targetPos, transform.position);

            if(targetDist > farRadius)
            {
                //Idle
            }

            if (farRadius > targetDist && targetDist > nearRadius && !isStaggered)
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
        isAttacking = false;
        canAttack = true;
    }


    public void resetBools()
    {
        canAttack = true;
        isAttacking = false;
        isStaggered = false;
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
