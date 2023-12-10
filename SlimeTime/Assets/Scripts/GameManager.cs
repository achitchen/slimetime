using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public Vector3 lastCheckPointPos;
    public Vector3 lastCheckPointCameraHolder;
    public bool isDead = false;
    public bool coreDestroyed1;
    public bool coreDestroyed2;
    public bool coreDestroyed3;
    public bool core3BossKilled1;
    public bool core3BossKilled2;
    public bool isQuitting = false;
    private bool canReload;
    public GameObject deathScreen;
    public GameObject deathText1;
    public GameObject deathText2;
    public GameObject deathImage;
    [SerializeField] GameObject initialCheckPoint;
    [SerializeField] GameObject initialCameraHolder;
    private GameManagerSounds gameManagerSounds;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        } 
        else
        {
            Destroy(gameObject);
        }
        lastCheckPointPos = initialCheckPoint.transform.position;
        lastCheckPointCameraHolder = initialCameraHolder.transform.position;
        coreDestroyed1 = false;
        coreDestroyed2 = false;
        coreDestroyed3 = false;
    }

    private void Start()
    {
        isDead = false;
        isQuitting = false;
        canReload = false;
        if (gameManagerSounds == null)
        {
            gameManagerSounds = GetComponent<GameManagerSounds>();
        }
        gameManagerSounds.bgmSoundSource.Play();
    }
    void Update()
    {
        if (isDead && canReload)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                deathScreen.SetActive(false);
                GetComponent<ProgressScript>().UpdateBools();
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                deathScreen = GameObject.Find("Death Panel");
                deathText1 = GameObject.Find("Death_Text1");
                deathText2 = GameObject.Find("Death_Text2");
                deathImage = GameObject.Find("Death_Image");
                isDead = false;
                canReload = false;
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isQuitting)
            {
                isQuitting = true;
                Invoke("CancelQuitting", 0.5f);
            }
            else if (isQuitting)
            {
                Application.Quit();
            }
        }
    }

    private void CancelQuitting()
    {
        isQuitting = false;
    }

    public void CanReload()
    {
        canReload = true;
    }

    public void ResetProgress()
    {
        coreDestroyed1 = false;
        coreDestroyed2 = false;
        coreDestroyed3 = false;
        core3BossKilled1 = false;
        core3BossKilled2 = false;
    }

    public void ActivateDeathScreen()
    {
        deathScreen.SetActive(true);
        deathScreen.GetComponent<Image>().canvasRenderer.SetAlpha(0);
        deathText1.GetComponent<TMP_Text>().canvasRenderer.SetAlpha(0);
        deathText2.GetComponent<TMP_Text>().canvasRenderer.SetAlpha(0);
        deathImage.GetComponent<Image>().canvasRenderer.SetAlpha(0);
        deathScreen.GetComponent<Image>().CrossFadeAlpha(1, 1, false);
        deathText1.GetComponent<TMP_Text>().CrossFadeAlpha(1, 1, false);
        deathText2.GetComponent<TMP_Text>().CrossFadeAlpha(1, 1, false);
        deathImage.GetComponent<Image>().CrossFadeAlpha(1, 1, false);
    }
}
