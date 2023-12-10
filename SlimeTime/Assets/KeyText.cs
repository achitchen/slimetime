using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KeyText : MonoBehaviour
{
    public int keyCount;
    [SerializeField] TMP_Text keyText;
    private ProgressScript progressScript;
    void Start()
    {
        progressScript = GameObject.Find("Game Manager").GetComponent<ProgressScript>();
        keyCount = progressScript.keysPossessed;
        keyText.SetText("x " + keyCount);
    }

    public void UpdateText()
    {
        keyCount = progressScript.keysPossessed;
        keyText.SetText("x " + keyCount);
    }
}
