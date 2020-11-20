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

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //Add Target
        if (!isTarget)
        {
            CreateTarget(new Vector3(Random.Range(-6, 9), Random.Range(1, 5), 0.0f));
        }

        PlayerControls();
        ShootingControls();
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
        particleManager.AddParticle(newTarget);
        newTarget.GetComponent<Target>().SetTargetVariables(newTarget);
        ForceGenerator2D bouyancyGenerator = forceManager.CreateBouyancyForceGenerator(newTarget, (waterSprite.transform.localScale.y) / 2.0f, 75.0f, 5.0f, -(waterSprite.transform.localScale.y) / 2.0f);
        forceManager.AddForceGenerator(bouyancyGenerator);
        newTarget.GetComponent<Target>().forceGen = bouyancyGenerator;

        //
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
                    particleManager.AddParticle(bullet);
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

                    //Create Bullets
                    GameObject springOne = Instantiate(springBullet);
                    GameObject springTwo = Instantiate(springBullet);

                    //Set Variables
                    springOne.GetComponent<Bullets>().SetBulletVariables(springOne, gunEnd);
                    springTwo.GetComponent<Bullets>().SetBulletVariables(springTwo, gunEnd);

                    //Transform Position
                    springOne.transform.position = gunEnd.transform.position;
                    springTwo.transform.position = gunEnd.transform.position;

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

                    //Particle Link
                    GameObject particleLinkRod = new GameObject("ParticleLink"); // Don't forget to make the rod u dumb dumb
                    Particle2DLink tempParticleLink = particleLinkRod.AddComponent<Particle2DLink>();
                    particleLink = tempParticleLink;

                    //Create Bullets
                    GameObject rodOne = Instantiate(rodBullet);
                    GameObject rodTwo = Instantiate(rodBullet);

                    //Set Variables
                    rodOne.GetComponent<Bullets>().SetBulletVariables(rodOne, gunEnd);
                    rodTwo.GetComponent<Bullets>().SetBulletVariables(rodTwo, gunEnd);

                    //Transform Position
                    rodOne.transform.position = new Vector2(gunEnd.transform.position.x, gunEnd.transform.position.y + 1.0f); //Change y so it doesn't spawn on top of each other
                    rodTwo.transform.position = gunEnd.transform.position;

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
}
