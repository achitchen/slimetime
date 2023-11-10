using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReloadAtCheckpoint : MonoBehaviour
{
    private GameManager gameManager;
    private GameObject mainCamera;
    public int slimeShellCountInit = 1;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        mainCamera = GameObject.Find("Main Camera");
        transform.position = gameManager.lastCheckPointPos;
        mainCamera.transform.position = gameManager.lastCheckPointCameraHolder;
        GetComponent<PlayerDodge>().slimeShellCount = slimeShellCountInit;
    }
}
