using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialScript : MonoBehaviour
{
    [SerializeField] GameObject returnPrompt;
    [SerializeField] GameObject tutorialPanel1;
    [SerializeField] GameObject tutorialPanel2;
    [SerializeField] GameObject tutorialPanel3;
    [SerializeField] GameObject tutorialPanel4;
    [SerializeField] GameObject tutorialPanel5;
    [SerializeField] GameObject key;
    private bool isKeyGiven;
    public bool isPageTurned;
    public bool canTurnPage;
    private bool isTutorialAvailable;
    private bool isTutorialActive;
    // Start is called before the first frame update
    void Start()
    {
        isKeyGiven = false;
        isPageTurned = false;
        returnPrompt.SetActive(false);
        isTutorialActive = false;
        canTurnPage = true;
        tutorialPanel1.SetActive(false);
    }

    private void Update()
    {
        if (isTutorialAvailable && !isTutorialActive)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                tutorialPanel1.SetActive(true);
                isTutorialActive = true;
                if (!isPageTurned)
                {
                    StartCoroutine(DelayPageTurn());
                }

            }
        }
        if (isTutorialActive)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (tutorialPanel1.activeSelf == true && canTurnPage)
                {
                    tutorialPanel2.SetActive(true);
                    tutorialPanel1.SetActive(false);
                    if (!isPageTurned)
                    {
                        StartCoroutine(DelayPageTurn());
                    }
                }
                else if (tutorialPanel2.activeSelf == true && canTurnPage)
                {
                    tutorialPanel3.SetActive(true);
                    tutorialPanel2.SetActive(false);
                    if (!isPageTurned)
                    {
                        StartCoroutine(DelayPageTurn());
                    }
                }
                else if (tutorialPanel3.activeSelf == true && canTurnPage)
                {
                    tutorialPanel4.SetActive(true);
                    tutorialPanel3.SetActive(false);
                    if (!isPageTurned)
                    {
                        StartCoroutine(DelayPageTurn());
                    }
                }
                else if (tutorialPanel4.activeSelf == true && canTurnPage)
                {
                    tutorialPanel5.SetActive(true);
                    tutorialPanel4.SetActive(false);
                    if (!isPageTurned)
                    {
                        StartCoroutine(DelayPageTurn());
                    }
                }
                else if (tutorialPanel5.activeSelf == true && canTurnPage)
                {
                    tutorialPanel5.SetActive(false);
                    if (!isKeyGiven)
                    {
                        key.SetActive(true);
                        isKeyGiven = true;
                    }
                    isTutorialActive = false;
                    if (!isPageTurned)
                    {
                        StartCoroutine(DelayPageTurn());
                    }
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            returnPrompt.SetActive(true);
            isTutorialAvailable = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            returnPrompt.SetActive(false);
            isTutorialAvailable = false;
        }
    }

    private IEnumerator DelayPageTurn()
    {
        canTurnPage = false;
        isPageTurned = true;
        yield return new WaitForSeconds(0.1f);
        canTurnPage = true;
        isPageTurned = false;
    }
}
