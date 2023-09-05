using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDodge : MonoBehaviour
{
    public Dodge dodgeActive;
    [SerializeField] float dodgeDuration = 0.3f;
    [SerializeField] float riposteDelay = 0.2f;
    [SerializeField] float riposteRecovery = 0.5f;
    [SerializeField] bool riposteReady = false;
    public bool riposteActivated = false;
    public bool slimeShellReady = false;
    public bool slimeShellActive = false;
    // Start is called before the first frame update
    void Start()
    {
        dodgeActive = Dodge.None;
        riposteActivated = false;
        riposteReady = false;
        slimeShellActive = false;
        slimeShellReady = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!riposteActivated)
        {
            DodgeInput();
        }

        if (riposteReady)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                StartCoroutine(RiposteActivate());
            }
        }

        if (slimeShellReady && !slimeShellActive)
        {
            if (Input.GetKeyDown(KeyCode.Y))
            {
                slimeShellActive = true;
            }
        }
    }

    private void DodgeInput()
    {
        if (dodgeActive == Dodge.None)
        {
            if (Input.GetKeyDown(KeyCode.RightShift))
            {
                StartCoroutine(DodgeTimer(Dodge.Horizontal));
            }
            else if (Input.GetKeyDown(KeyCode.Return))
            {
                StartCoroutine(DodgeTimer(Dodge.Vertical));
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine(DodgeTimer(Dodge.Absorb));
            }
        }
    }

    public enum Dodge 
    {
    None,
    Horizontal,
    Vertical,
    Absorb
    }

    private IEnumerator DodgeTimer(Dodge dodge)
    {
        dodgeActive = dodge;
        yield return new WaitForSeconds(dodgeDuration);
        dodgeActive = Dodge.None;
    }

    private IEnumerator RiposteActivate()
    {
        if (!riposteActivated)
        {
            riposteActivated = true;
            yield return new WaitForSeconds(riposteDelay);
            riposteReady = false;
            yield return new WaitForSeconds(riposteRecovery);
            riposteActivated = false;
        }
    }
}
