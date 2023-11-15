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
    public bool coreOneDestroyed;
    public bool coreTwoDestroyed;
    public bool coreThreeDestroyed;
    [SerializeField] GameObject initialCheckPoint;
    [SerializeField] GameObject initialCameraHolder;

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
        coreOneDestroyed = false;
        coreTwoDestroyed = false;
        coreThreeDestroyed = false;

    }

    private void Start()
    {
        isDead = false;
    }
    void Update()
    {
        if (isDead)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                isDead = false;
            }
        }
    }
}
