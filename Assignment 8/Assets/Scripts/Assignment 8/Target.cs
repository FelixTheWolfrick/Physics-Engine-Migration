using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    //Variables
    public ForceGenerator2D forceGen;
    GameManager manager;
    GameObject[] bullets;

    // Start is called before the first frame update
    void Start()
    {
        forceGen = gameObject.GetComponent<ForceGenerator2D>();
        //manager = GameObject.Find("Manager").GetComponent<Controls>();
    }

    // Update is called once per frame
    void Update()
    {
        //DeleteTarget();
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

    public void DeleteTarget()
    {
        //Get all bullets on screen
        bullets = GameObject.FindGameObjectsWithTag("Bullet");

        foreach (GameObject bullet in bullets)
        {
            if (Vector2.Distance(bullet.transform.position, gameObject.transform.position) < 1.2)
            {
                manager.GetComponent<GameManager>().scoreText.text = manager.GetComponent<GameManager>().score.ToString();
                Destroy(gameObject);
            }
        }
    }
}
