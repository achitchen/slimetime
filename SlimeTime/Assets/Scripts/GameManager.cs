using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
                GetComponent<ProgressScript>().UpdateBools();
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
}
