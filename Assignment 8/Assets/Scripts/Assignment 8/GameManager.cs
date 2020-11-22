using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
//using System.Numerics;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //Variables
    public float rotateSpeed = 2.0f;
    public GameObject player;

    public GameObject gunEnd;
    public GameObject bullet;
    public GameObject springBullet;
    public GameObject rodBullet;
    private int bulletNum = 1; //Which Bullet Selected

    public GameObject waterSprite;
    public ForceManager forceManager;
    public ParticleManager particleManager;
    public Particle2DLink particleLink;
    public Particle2DContact particleContact;

    public GameObject target;
    public bool isTarget;

    public Text scoreText;
    public int score;

    public float timeLeft = 2.0f;
    public GameObject randomParticle;
    public GameObject[] totalRandomParticles;
    public int maxParticles = 3; //Max amount of particles allowed on screen at once
    public Text timerText;

    // Start is called before the first frame update
    void Start()
    {
        score = -1;
    }

    // Update is called once per frame
    void Update()
    {
        //Add Target
        if (!isTarget)
        {
            CreateTarget(new Vector3(Random.Range(-6, 9), Random.Range(1, 5), 0.0f));
        }

        //Controls
        PlayerControls();
        ShootingControls();

        //Generate Random Particle
        GenerateRandomParticle();
    }

    //Player Controls
    void PlayerControls()
    {
        //Check if keys are pressed
        if (Input.GetKey(KeyCode.Alpha1) == true) //Left
        {
            player.transform.Rotate(0, 0, 180 * rotateSpeed * Time.deltaTime);

        }
        else if (Input.GetKey(KeyCode.Alpha2) == true) //Right
        {
            player.transform.Rotate(0, 0, -180 * rotateSpeed * Time.deltaTime);
        }
    }

    //Create Target
    void CreateTarget(Vector3 targetPosition)
    {
        //Create Target & Set Position
        GameObject newTarget = Instantiate(target);
        //newTarget.transform.position = new Vector3(-2.75f, 3.26f, 0.0f);
        newTarget.transform.position = targetPosition;

        //Set Variables
        //particleManager.AddParticle(newTarget);
        newTarget.GetComponent<Target>().SetTargetVariables(newTarget);
        ForceGenerator2D bouyancyGenerator = forceManager.CreateBouyancyForceGenerator(newTarget, (waterSprite.transform.localScale.y) / 2.0f, 75.0f, 5.0f, -(waterSprite.transform.localScale.y) / 2.0f);
        forceManager.AddForceGenerator(bouyancyGenerator);
        newTarget.GetComponent<Target>().forceGen = bouyancyGenerator;

        //Update score
        score++;
        scoreText.text = "Score: " + score;
        isTarget = true;
    }

    //Shooting Bullets Controls
    void ShootingControls()
    {
        //W - Change Bullet
        if (Input.GetKeyDown(KeyCode.W) == true)
        {
            if (bulletNum >= 3) //Reset back to one if reaches 3
            {
                bulletNum = 1;
            }
            else
            {
                bulletNum += 1;
            }
            Debug.Log("Bullet: " + bulletNum);
        }

        //Creat Bullet
        if (Input.GetKeyDown(KeyCode.Return) == true)
        {
            switch (bulletNum)
            {
                case 1: //Bullet
                    Debug.Log("Bullet");

                    //Create Bullet
                    GameObject bulletOne = Instantiate(bullet);
                    //bulletOne.transform.position = gunEnd.transform.position;

                    //Set Variables
                    //particleManager.AddParticle(bullet);
                    bulletOne.GetComponent<Bullets>().SetBulletVariables(bulletOne, gunEnd);

                    //Transform Position
                    bulletOne.transform.position = gunEnd.transform.position;
                    bulletOne.transform.rotation = gunEnd.transform.rotation;

                    //Force Generator
                    bulletOne.GetComponent<Bullets>().isForceGen = true;
                    ForceGenerator2D BouyancyGen2 = forceManager.CreateBouyancyForceGenerator(bulletOne, (waterSprite.transform.localScale.y) / 2.0f, 75.0f, 5.0f, -(waterSprite.transform.localScale.y) / 2.0f);
                    forceManager.AddForceGenerator(BouyancyGen2);
                    bulletOne.GetComponent<Bullets>().forceGen = BouyancyGen2;

                    break;
                case 2: //Spring
                    Debug.Log("Spring");

                    //Set Value
                    int numSprings = 1;

                    //Create Bullets
                    GameObject springOne = Instantiate(springBullet);
                    springOne.name = "Spring" + numSprings;
                    numSprings++;
                    GameObject springTwo = Instantiate(springBullet);
                    springTwo.name = "Spring" + numSprings;
                    numSprings++;

                    //Set Variables
                    springOne.GetComponent<Bullets>().SetBulletVariables(springOne, gunEnd);
                    springTwo.GetComponent<Bullets>().SetBulletVariables(springTwo, gunEnd);

                    //Transform Position
                    springOne.transform.position = new Vector2(gunEnd.transform.position.x, gunEnd.transform.position.y);
                    springTwo.transform.position = gunEnd.transform.position;
                    springOne.transform.rotation = gunEnd.transform.rotation;
                    springTwo.transform.rotation = gunEnd.transform.rotation;

                    //Force Generator
                    springOne.GetComponent<Bullets>().isForceGen = true;
                    ForceGenerator2D springGenerator = forceManager.CreateSpringForceGenerator(springOne, springTwo, 1.0f, 10.0f);
                    forceManager.AddForceGenerator(springGenerator);
                    springOne.GetComponent<Bullets>().forceGen = springGenerator;

                    springOne.GetComponent<Bullets>().isForceGen = true;
                    ForceGenerator2D BouyancyGenerator = forceManager.CreateBouyancyForceGenerator(springOne, (waterSprite.transform.localScale.y) / 2.0f, 75.0f, 5.0f, -(waterSprite.transform.localScale.y) / 2.0f);
                    forceManager.AddForceGenerator(BouyancyGenerator);
                    springOne.GetComponent<Bullets>().forceGen = BouyancyGenerator;

                    break;
                case 3: //Rod
                    Debug.Log("Rod");

                    //Set Value
                    int numRods = 1;

                    //Particle Link
                    GameObject particleLinkRod = new GameObject("ParticleLink"); // Don't forget to make the rod u dumb dumb
                    Particle2DLink tempParticleLink = particleLinkRod.AddComponent<Particle2DLink>();
                    particleLink = tempParticleLink;

                    //Create Bullets
                    GameObject rodOne = Instantiate(rodBullet);
                    rodOne.name = "Rod" + numRods;
                    numRods++;
                    GameObject rodTwo = Instantiate(rodBullet);
                    rodTwo.name = "Rod" + numRods;
                    numRods++;

                    //Set Variables
                    rodOne.GetComponent<Bullets>().SetBulletVariables(rodOne, gunEnd);
                    rodTwo.GetComponent<Bullets>().SetBulletVariables(rodTwo, gunEnd);

                    //Transform Position
                    rodOne.transform.position = new Vector2(gunEnd.transform.position.x, gunEnd.transform.position.y + 1.0f); //Change y so it doesn't spawn on top of each other
                    rodTwo.transform.position = gunEnd.transform.position;
                    rodOne.transform.rotation = gunEnd.transform.rotation;
                    rodTwo.transform.rotation = gunEnd.transform.rotation;

                    //Particle Link
                    rodOne.GetComponent<Bullets>().isParticleLink = true;
                    Particle2DLink link = particleLink.CreateLink(rodOne, rodTwo, 1.0f);
                    rodOne.GetComponent<Bullets>().particleLink = link;

                    //Force Generator
                    rodOne.GetComponent<Bullets>().isForceGen = true;
                    ForceGenerator2D BouyancyGen = forceManager.CreateBouyancyForceGenerator(rodOne, (waterSprite.transform.localScale.y) / 2.0f, 75.0f, 5.0f, -(waterSprite.transform.localScale.y) / 2.0f);
                    forceManager.AddForceGenerator(BouyancyGen);
                    rodOne.GetComponent<Bullets>().forceGen = BouyancyGen;

                    break;
            }
        }
    }

    void GenerateRandomParticle()
    {
        //Update Time Left
        timeLeft -= Time.deltaTime;
        timerText.text = "Time Left Until Next Particle: " + Mathf.Round(timeLeft) + " seconds";

        //Get Total Random Particles On Screen
        totalRandomParticles = GameObject.FindGameObjectsWithTag("RandomParticle");

        //Generate Random Particle When Time Hits 0
        if (timeLeft <= 0 && totalRandomParticles.Length <= maxParticles)
        {
            Debug.Log("Generating Random Particle");

            //Create Particle
            GameObject randomParticleGen = Instantiate(randomParticle);

            //Set Variables
            randomParticleGen.GetComponent<Target>().SetTargetVariables(randomParticleGen);

            //Transform Position
            randomParticleGen.transform.position = new Vector2(Random.Range(-6, 9), Random.Range(1, 5));

            //Force Generator
            ForceGenerator2D BouyancyGenRandomParticle = forceManager.CreateBouyancyForceGenerator(randomParticleGen, (waterSprite.transform.localScale.y) / 2.0f, 75.0f, 5.0f, -(waterSprite.transform.localScale.y) / 2.0f);
            forceManager.AddForceGenerator(BouyancyGenRandomParticle);
            randomParticleGen.GetComponent<Target>().forceGen = BouyancyGenRandomParticle;

            //Reset Time Left
            timeLeft = 30.0f;
        }
    }
}
