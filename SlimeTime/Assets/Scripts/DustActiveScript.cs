using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustActiveScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 0.35f);
    }
}
