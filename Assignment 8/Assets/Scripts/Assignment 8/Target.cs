using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    //Variables
    public ForceGenerator2D forceGen;
    GameManager manager;
    GameObject[] randomParticles;

    public GameObject topBoundry;
    public GameObject bottomBoundry;
    public GameObject rightBoundry;
    public GameObject leftBoundry;

    // Start is called before the first frame update
    void Start()
    {
        forceGen = gameObject.GetComponent<ForceGenerator2D>();
        //manager = GameObject.Find("Manager").GetComponent<Controls>();

        //Set Boundries
        topBoundry = GameObject.Find("TopBoundry");
        bottomBoundry = GameObject.Find("BottomBoundry");
        leftBoundry = GameObject.Find("LeftBoundry");
        rightBoundry = GameObject.Find("RightBoundry");
    }

    // Update is called once per frame
    void Update()
    {
        CheckBoundries();
        CollisionWithRandomParticle();
    }

    //Set Variables
    public void SetTargetVariables(GameObject target)
    {
        Particle2D stats = target.GetComponent<Particle2D>();
        stats.speed = 50.0f;
        stats.acceleration = new Vector2(0.0f, -20.0f);
        stats.velocity = target.transform.forward * stats.speed;
        stats.dampingCost = 0.99f;
    }

    //This is a really round about way of trying to stop the target from going up so high
    //in the air since I can't figure out why my forces affect everything everytime a new bullet is shot
    //Hopefully this makes hitting and waiting for the target easier
    public void CheckBoundries()
    {
        if(gameObject.transform.position.y > topBoundry.transform.position.y) //Check X Boundry
        {
            gameObject.transform.position = new Vector2(gameObject.transform.position.x, topBoundry.transform.position.y - 2);
            forceGen.resetForce(gameObject);
        }
        else if(gameObject.transform.position.y < bottomBoundry.transform.position.y)
        {
            gameObject.transform.position = new Vector2(gameObject.transform.position.x, bottomBoundry.transform.position.y + 2);
            forceGen.resetForce(gameObject);
        }
    }

    public void CollisionWithRandomParticle()
    {
        //If it hits a random target delete it
        randomParticles = GameObject.FindGameObjectsWithTag("RandomParticle");

        foreach (GameObject randomParticleHit in randomParticles)
        {
            if (gameObject != randomParticleHit)
            {
                if (Vector2.Distance(randomParticleHit.transform.position, gameObject.transform.position) < 1.2)
                {
                    Debug.Log("Hit the target");
                    Destroy(gameObject);
                    Destroy(randomParticleHit);
                }
            }
        }
    }
}
