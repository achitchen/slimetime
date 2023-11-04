using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReloadAtCheckpoint : MonoBehaviour
{
    private GameManager gameManager;
    public int slimeShellCountInit = 1;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        transform.position = gameManager.lastCheckPointPos;
        GetComponent<PlayerDodge>().slimeShellCount = slimeShellCountInit;
    }
}
