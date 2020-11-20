using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle2D : MonoBehaviour
{
    //Variables
    public float mass;
    public Vector2 velocity;
    public Vector2 acceleration;
    public Vector2 accumulatedForces;
    public float dampingCost;
    public float facing;
    public float speed;
    public bool ignoreForces;
    public Integrator integrator;

    // Start is called before the first frame update
    void Start()
    {
        integrator = GameObject.Find("Integrator").GetComponent<Integrator>();
    }

    // Update is called once per frame
    void Update()
    {
        integrator.integrate(gameObject, Time.deltaTime);
    }
}
