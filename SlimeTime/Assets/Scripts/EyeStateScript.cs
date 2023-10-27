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
    private GameObject player;
    private bool canShoot = true;
    private bool canBite = true;
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
        targetPos = player.transform.position;
        targetDist = Vector3.Distance(targetPos, transform.position);

        if (targetDist > farRadius)
        {
            moveDir = (targetPos - transform.position).normalized;
            transform.Translate(moveDir * speed * Time.deltaTime);
        }
        else if (farRadius >= targetDist && targetDist >= midRadius)
        {

            RangedAttack();

        }
        else if (midRadius >= targetDist && targetDist > nearRadius)
        {
            moveDir = (targetPos - transform.position).normalized;
            transform.Translate(moveDir * speed * Time.deltaTime);
        }
        else
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
        yield return new WaitForSeconds(projectileRecharge);
        canShoot = true;
    }

    private IEnumerator BiteWindup()
    {
        //show hitbox/telegraphing
        moveDir = Vector3.zero;
        yield return new WaitForSeconds(biteDelay);
        //make attack hitbox active
        //do attack animation
        Debug.Log("Bite!");
        canBite = true;
    }
}
