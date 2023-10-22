using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState : MonoBehaviour
{
    public enum EnemyStates
    {
        Idle,
        Chase,
        Attack
    }

    [SerializeField] EnemyStates currentState = EnemyStates.Idle;
    [SerializeField] GameObject questionMark;
    [SerializeField] GameObject exclamationMark;
    private bool isAlerted = false;

    public void Update()
    {
        switch (currentState)
        {
            case EnemyStates.Idle:
                //do nothing;
                break;
            case EnemyStates.Chase:
                //chase player;
                break;
            case EnemyStates.Attack:
                //do attack patterns;
                break;
        }
    }
}
