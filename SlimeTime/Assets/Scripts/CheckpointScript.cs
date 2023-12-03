using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointScript : MonoBehaviour
{
    private GameManager gameManager;
    [SerializeField] GameObject cameraHolder;
    private void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            gameManager.lastCheckPointPos = transform.position;
            gameManager.lastCheckPointCameraHolder = cameraHolder.transform.position;
            collision.GetComponent<PlayerDodge>().slimeShellCount = 1;
        }
    }
}
