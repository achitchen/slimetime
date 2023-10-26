using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeStateScript : MonoBehaviour
{
    [SerializeField] GameObject questionMark;
    [SerializeField] GameObject exclamationMark;
    [SerializeField] GameObject projectile;
    [SerializeField] int nearRadius = 1;
    [SerializeField] int midRadius = 3;
    [SerializeField] int farRadius = 5;
    [SerializeField] int speed = 2;
    [SerializeField] int projectileSpeed = 1;
    [SerializeField] float targetDist;
    [SerializeField] float projectileRecharge = 2f;
    private GameObject player;
    private bool canShoot = true;
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
            moveDir = Vector3.zero;
            RangedAttack();
        }
        else if (midRadius >= targetDist && targetDist > nearRadius)
        {
            moveDir = (targetPos - transform.position).normalized;
            transform.Translate(moveDir * speed * Time.deltaTime);
        }
        else
        {
            moveDir = Vector3.zero;
            //do melee attack
            Debug.Log("Bite!");
        }
    }



    private void RangedAttack()
    {
        {
            if (canShoot)
            {
                targetPos = player.transform.position;
                GameObject clone = Instantiate(projectile, transform.position, transform.rotation);
                clone.GetComponent<Rigidbody2D>().AddForce((targetPos - transform.position).normalized * projectileSpeed, ForceMode2D.Impulse);
                StartCoroutine("RechargeShot");
            }
        }
    }
    private IEnumerator RechargeShot()
    {
        canShoot = false;
        moveDir = (targetPos - transform.position).normalized;
        yield return new WaitForSeconds(projectileRecharge);
        canShoot = true;
    }
}
