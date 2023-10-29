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
    [SerializeField] float staggerDuration = 1f;
    [SerializeField] GameObject biteTelegraph;
    [SerializeField] GameObject biteAttack;
    private GameObject player;
    private bool canShoot = true;
    private bool canBite = true;
    private bool isAttacking = false;
    private Vector3 moveDir;
    private Vector3 targetPos;


    private void Start()
    {
        if (player == null)
        {
            player = GameObject.Find("Player");

        }
    }

    private void Update()
    {
        bool isStaggered = gameObject.GetComponent<EnemyHealth>().isStaggered;
        targetPos = player.transform.position;
        targetDist = Vector3.Distance(targetPos, transform.position);

        if (targetDist > farRadius && !isStaggered)
        {
            if (!isAttacking)
            {
                moveDir = (targetPos - transform.position).normalized;
                transform.Translate(moveDir * speed * Time.deltaTime);
            }
        }
        else if (farRadius >= targetDist && targetDist >= midRadius && !isStaggered)
        {
                RangedAttack();
        }
        else if (midRadius >= targetDist && targetDist > nearRadius)
        {
            if (player.GetComponent<PlayerDodge>().riposteReady)
            {
                player.GetComponent<PlayerDodge>().riposteTargets.Remove(gameObject);
                if (player.GetComponent<PlayerDodge>().riposteTargets.Count == 0)
                {
                    player.GetComponent<PlayerDodge>().riposteReady = false;
                }
            }
            if (!isAttacking && !isStaggered)
            {
                moveDir = (targetPos - transform.position).normalized;
                transform.Translate(moveDir * speed * Time.deltaTime);
            }
        }
        else if (GetComponent<EnemyHealth>().canBeRiposted)
        {
            if (!player.GetComponent<PlayerDodge>().riposteReady)
            {
                player.GetComponent<PlayerDodge>().riposteReady = true;
                player.GetComponent<PlayerDodge>().riposteTargets.Add(gameObject);
                if (!isStaggered) 
                {
                    StartCoroutine("EnemyStaggered");
                }
                
            }
            
        }
        else if (!isStaggered)
        {
            MeleeAttack();
        }
    }

    private void MeleeAttack()
    {
        if (canBite)
        {
            StartCoroutine("BiteWindup");
            canBite = false;
        }
    }

    private void RangedAttack()
    {
        {
            if (canShoot)
            {
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
        float newDirX = Random.Range(-5, 5);
        float newDirY = Random.Range(-5, 5);
        moveDir = new Vector2(targetPos.x + newDirX, targetPos.y + newDirY).normalized;
        isAttacking = false;
        yield return new WaitForSeconds(projectileRecharge);
        canShoot = true;
    }

    private IEnumerator BiteWindup()
    {
        isAttacking = true;
        Vector2 attackPos = targetPos;
        biteTelegraph.SetActive(true);
        biteTelegraph.transform.position = attackPos;
        moveDir = Vector3.zero;
        yield return new WaitForSeconds(biteDelay);
        biteTelegraph.SetActive(false);
        biteAttack.SetActive(true);
        biteAttack.transform.position = attackPos;
        Debug.Log("Bite!");
        yield return new WaitForSeconds(biteActive);
        biteAttack.SetActive(false);
        isAttacking = false;
        canBite = true;
    }

    public IEnumerator EnemyStaggered()
    {
        Debug.Log("Enemy is reeling! Riposte Ready!");
        gameObject.GetComponent<EnemyHealth>().isStaggered = true;
        yield return new WaitForSeconds(staggerDuration);
        gameObject.GetComponent<EnemyHealth>().isStaggered = false;
        player.GetComponent<PlayerDodge>().riposteTargets.Remove(gameObject);
        gameObject.GetComponent<EnemyHealth>().currentHits--;
        gameObject.GetComponent<EnemyHealth>().canBeRiposted = false;
    }
}
