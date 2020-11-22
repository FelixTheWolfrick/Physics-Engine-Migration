using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Bullets : MonoBehaviour
{
    //Variables
    public ForceGenerator2D forceGen = null;
    public Particle2DLink particleLink = null;
    public bool isForceGen = false;
    public bool isParticleLink = false;

    public GameObject topBoundry;
    public GameObject bottomBoundry;
    public GameObject rightBoundry;
    public GameObject leftBoundry;

    GameObject[] randomParticles;
    GameObject[] targets;
    public GameObject gameManager;

    // Start is called before the first frame update
    void Start()
    {
        //Set Boundries
        topBoundry = GameObject.Find("TopBoundry");
        bottomBoundry = GameObject.Find("BottomBoundry");
        leftBoundry = GameObject.Find("LeftBoundry");
        rightBoundry = GameObject.Find("RightBoundry");

        //Set Game Manager
        gameManager = GameObject.Find("Player");

        //Get Components
        if (forceGen)
        {
            forceGen = gameObject.GetComponent<ForceGenerator2D>();
        }

        if(particleLink)
        {
            particleLink = gameObject.GetComponent<Particle2DLink>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        DeleteBullet();
    }

    //Set Variables
    public void SetBulletVariables(GameObject bullet, GameObject gun)
    {
        Particle2D stats = bullet.GetComponent<Particle2D>();
        stats.speed = 50.0f;
        stats.acceleration = new Vector2(0.0f, -5.0f);
        Vector3 direction = new Vector3((float)Mathf.Cos(gun.transform.rotation.z), (float)Mathf.Sin(gun.transform.rotation.z), 0.0f);
        stats.velocity = direction * stats.speed;
        stats.dampingCost = 0.99f;
    }

    //Delete if off screen
    void DeleteBullet() 
    {
        //If it's past the screen, delete it
        if(gameObject.transform.position.y > topBoundry.transform.position.y || gameObject.transform.position.y < bottomBoundry.transform.position.y
            || gameObject.transform.position.x < leftBoundry.transform.position.x || gameObject.transform.position.x > rightBoundry.transform.position.x)
        { //This can definitely be done better but ya know what it works and I'm tired
            Destroy(gameObject);
        }

        //If it hits a taret delete it
        targets = GameObject.FindGameObjectsWithTag("Target");

        foreach (GameObject target in targets)
        {
            if (gameObject != target)
            {
                if (Vector2.Distance(target.transform.position, gameObject.transform.position) < 1.2)
                {
                    Debug.Log("Hit the target");
                    gameManager.GetComponent<GameManager>().isTarget = false;
                    //Destroy(target);
                    Destroy(gameObject);
                    Destroy(target);
                }
            }
        }

        //If it hits a random particle delete it
        randomParticles = GameObject.FindGameObjectsWithTag("RandomParticle");

        foreach (GameObject randomParticle in randomParticles)
        {
            if (gameObject != randomParticle)
            {
                if (Vector2.Distance(randomParticle.transform.position, gameObject.transform.position) < 1.2)
                {
                    Debug.Log("Hit a random particle");
                    //Destroy(target);
                    Destroy(gameObject);
                    Destroy(randomParticle);
                }
            }
        }
    }
}
