using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomParticlesControls : MonoBehaviour
{
    public ParticleManager particleManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        DeleteRandomParticles();
    }

    //Delete Random Particles
    void DeleteRandomParticles()
    {
        if (Input.GetKeyDown(KeyCode.E) == true)
        {
            Destroy(gameObject);
        }
    }
}
