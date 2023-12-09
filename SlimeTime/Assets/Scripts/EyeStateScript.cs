using System.Collections;
using UnityEngine;

public class EyeStateScript : MonoBehaviour
{
    [SerializeField] GameObject projectile;
    [SerializeField] int nearRadius = 1;
    [SerializeField] int midRadius = 3;
    [SerializeField] int farRadius = 5;
    [SerializeField] int speed = 2;
    [SerializeField] int projectileSpeed = 1;
    [SerializeField] float targetDist;
    [SerializeField] float projectileRecharge = 2f;
    [SerializeField] float biteDelay = 1f;
    [SerializeField] float biteActive = 0.5f;
    [SerializeField] float biteRecovery = 0.75f;
    [SerializeField] GameObject biteTelegraph;
    [SerializeField] GameObject horizontalAttack;
    [SerializeField] GameObject reflectiveAttack;
    [SerializeField] GameObject spriteObject;
    [SerializeField] GameObject horizontalIcon;
    [SerializeField] GameObject shieldIcon;
    private GameObject player;
    private Animator animator;
    private EnemySounds enemySounds;
    private bool canShoot = true;
    private bool canBite = true;
    private bool isAttacking = false;
    private bool isStaggered;
    private Vector3 moveDir;
    private Vector3 targetPos;


    private void Start()
    {
        if (player == null)
        {
            player = GameObject.Find("Player");
            Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }
        animator = GetComponent<Animator>();
        if (enemySounds == null)
        {
            enemySounds = GetComponent<EnemySounds>();
        }
    }

    private void Update()
    {
        isStaggered = gameObject.GetComponent<EnemyHealth>().isStaggered;
        if (player != null && !player.GetComponent<PlayerDodge>().riposteActivated)
        {
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
                {
                    targetPos = player.transform.position;
                    targetDist = Vector3.Distance(targetPos, transform.position);

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
                    else if (farRadius >= targetDist && targetDist >= midRadius && !isStaggered)
                    {
                        RangedAttack();
                    }
                    else if (midRadius >= targetDist && targetDist > nearRadius)
                    {
                        if (!isAttacking && !isStaggered)
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
                    else if (!isStaggered)
                    {
                        MeleeAttack();
                    }
                }
                if (GetComponent<EnemyHealth>().canBeRiposted)
                {
                    animator.SetTrigger("hitTrigger");
                    StopAllCoroutines();
                    if (!isStaggered)
                    {
                        GetComponent<EnemyHealth>().StartCoroutine("EnemyStaggered");
                        animator.ResetTrigger("runTrigger");
                        animator.ResetTrigger("idleTrigger");
                        animator.ResetTrigger("attackTrigger");
                        animator.ResetTrigger("windupTrigger");
                    }
                }
            }
        }
    }

    private void MeleeAttack()
    {
        if (canBite)
        {
            int attackIndex = Random.Range(1, 100);
            if (attackIndex < 51)
            {
                StartCoroutine(BiteWindup(AttackType.horizontal));
            }
            else
            {
                StartCoroutine(BiteWindup(AttackType.reflect));
            }
            canBite = false;
        }
    }

    private void RangedAttack()
    {
        {
            if (canShoot)
            {
                animator.ResetTrigger("runTrigger");
                animator.SetTrigger("attackTrigger");
                isAttacking = true;
                moveDir = Vector3.zero;
                GameObject clone = Instantiate(projectile, transform.position, transform.rotation);
                clone.GetComponent<Rigidbody2D>().AddForce((targetPos - transform.position).normalized * projectileSpeed, ForceMode2D.Impulse);
                StartCoroutine("RechargeShot");
            }
            else
            {
                transform.Translate(moveDir * speed * Time.deltaTime);
            }
        }
    }
    private IEnumerator RechargeShot()
    {
        canShoot = false;
        yield return new WaitForSeconds(1f);
        animator.ResetTrigger("attackTrigger");
        animator.SetTrigger("runTrigger");
        float newDirX = Random.Range(-5, 5);
        float newDirY = Random.Range(-5, 5);
        moveDir = new Vector2(targetPos.x + newDirX, targetPos.y + newDirY).normalized;
        isAttacking = false;
        yield return new WaitForSeconds(projectileRecharge);
        canShoot = true;
        
    }

    private IEnumerator BiteWindup(AttackType attackType)
    {
        animator.ResetTrigger("runTrigger");
        animator.SetTrigger("windupTrigger");
        animator.ResetTrigger("idleTrigger");
        animator.ResetTrigger("attackTrigger");
        animator.ResetTrigger("hitTrigger");
        if (attackType == AttackType.horizontal)
        {
            enemySounds.enemySoundSource.PlayOneShot(enemySounds.horizontalWindupSound);
            horizontalIcon.SetActive(true);
            horizontalIcon.GetComponent<SpriteRenderer>().flipX = spriteObject.GetComponent<SpriteRenderer>().flipX;
        }
        else if (attackType == AttackType.reflect)
        {
            enemySounds.enemySoundSource.PlayOneShot(enemySounds.reflectWindupSound);
            shieldIcon.SetActive(true);
        }
        isAttacking = true;
        Vector2 attackPos = targetPos;
        biteTelegraph.SetActive(true);
        biteTelegraph.transform.position = attackPos;
        moveDir = Vector3.zero;
        yield return new WaitForSeconds(biteDelay);
        animator.ResetTrigger("windupTrigger");
        animator.SetTrigger("attackTrigger");
        biteTelegraph.SetActive(false);
        horizontalIcon.SetActive(false);
        shieldIcon.SetActive(false);
        if (attackType == AttackType.horizontal)
        {
            horizontalAttack.SetActive(true);
            horizontalAttack.transform.position = attackPos;
        }
        else if (attackType == AttackType.reflect)
        {
            reflectiveAttack.SetActive(true);
            reflectiveAttack.transform.position = attackPos;
        }
        enemySounds.enemySoundSource.PlayOneShot(enemySounds.bluntAttackSound);
        Debug.Log("Bite!");
        animator.ResetTrigger("attackTrigger");
        animator.SetTrigger("idleTrigger");
        yield return new WaitForSeconds(biteActive);
        horizontalAttack.SetActive(false);
        reflectiveAttack.SetActive(false);
        isAttacking = false;
        yield return new WaitForSeconds(biteRecovery);
        canBite = true;
    }

    public void resetBools()
    {
        canShoot = true;
        canBite = true;
        isAttacking = false;
        isStaggered = false;
        animator.ResetTrigger("idleTrigger");
        animator.ResetTrigger("attackTrigger");
        animator.ResetTrigger("windupTrigger");
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

    private enum AttackType
    {
        horizontal,
        vertical,
        reflect
    }
}
