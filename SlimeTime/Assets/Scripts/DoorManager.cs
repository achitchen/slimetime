using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    [SerializeField] GameObject destination;
    [SerializeField] GameObject cameraHolder;
    [SerializeField] GameObject mainCamera;

    private void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = GameObject.Find("Main Camera");
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameObject player = collision.gameObject;
            player.transform.position = destination.transform.position;
            mainCamera.transform.position = cameraHolder.transform.position;
        }
    }
}
