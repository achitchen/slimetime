using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustParticleScript : MonoBehaviour
{
    public GameObject dustParticle;
    // Start is called before the first frame update
    public void ActivateDust()
    {
        Instantiate(dustParticle, transform.position, transform.rotation);
    }
}
