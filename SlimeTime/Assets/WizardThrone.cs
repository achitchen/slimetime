using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WizardThrone : MonoBehaviour
{
    [SerializeField] GameObject returnIcon;
    [SerializeField] GameObject endText;
    private bool gameEnded;
    private bool canEnd;
    private GameManager gameManager;
    private ProgressScript progressScript;
    // Start is called before the first frame update
    void Start()
    {
        gameEnded = false;
        if (gameManager == null)
        {
            gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        }
        if (progressScript == null)
        {
            progressScript = GameObject.Find("Game Manager").GetComponent<ProgressScript>();
        }
    }

    private void Update()
    {
        if (canEnd &! gameEnded)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                endText.SetActive(true);
                canEnd = false;
                gameEnded = true;
                Invoke("ReturnPressed", 0.5f);
            }
        }
        else if (gameEnded && canEnd)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                endText.SetActive(false);
                gameEnded = false;
                progressScript.ResetProgress();
                gameManager.ResetProgress();
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        returnIcon.SetActive(true);
        canEnd = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        returnIcon.SetActive(false);
        canEnd = false;
    }

    private void ReturnPressed()
    {
        canEnd = true;
    }
}
