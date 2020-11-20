using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;

public class Integrator : MonoBehaviour
{
    //Variables
    private static Integrator instance;
    public static Integrator Instance { get { return instance; } }
    public List<Particle2D> mParticles = new List<Particle2D>();

    Particle2D particle;

    //Called when first loaded
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //Integrate Particle2D
    public void integrate(GameObject particleObj, double dt)
    {
        //Variables
        particle = particleObj.GetComponent<Particle2D>();
        particleObj.transform.position += new Vector3((particle.velocity.x * (float)dt), (particle.velocity.y * (float)dt), 0.0f);
        Vector2 resultingAcc = particle.acceleration;

        //Calculate
        if(!particle.ignoreForces)
        {
            resultingAcc += (particle.accumulatedForces * (float)(particle.mass / 1.0));
        }

        particle.velocity += (resultingAcc * (float)dt);
        float damping = (float)Math.Pow((float)particle.dampingCost, (float)dt);
        particle.velocity *= damping;

        particle.accumulatedForces = Vector2.zero;
    }
}
