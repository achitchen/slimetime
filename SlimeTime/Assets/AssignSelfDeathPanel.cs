using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssignSelfDeathPanel : MonoBehaviour
{
    [SerializeField] GameObject deathText1;
    [SerializeField] GameObject deathText2;
    [SerializeField] GameObject deathImage;
    private GameManager gameManager;
    
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        gameManager.deathScreen = gameObject;
        gameManager.deathText1 = deathText1;
        gameManager.deathText2 = deathText2;
        gameManager.deathImage = deathImage;
        gameObject.SetActive(false);
    }
}
