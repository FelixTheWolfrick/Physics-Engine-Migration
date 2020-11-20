using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    //Variables
    private static Particle2D instance;
    private static Particle2D Instance { get { return instance; } }

    List<GameObject> aliveParticles = new List<GameObject>();
    List<GameObject> deadParticles = new List<GameObject>();
    //Controls manager;
    public GameObject manager;
    //Target targetHit;
    GameObject[] bullets;

    // Start is called before the first frame update
    void Start()
    {
        //manager = GameObject.Find("Manager").GetComponent<Controls>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateParticles();
    }

    //Add Particle
    public void AddParticle(GameObject particle)
    {
        aliveParticles.Add(particle);
    }

    //Remove Particle
    public void RemoveParticle(GameObject particle)
    {
        if(particle && particle.tag == "Target")
        {
            manager.GetComponent<GameManager>().isTarget = false;
        }

        deadParticles.Remove(particle);
        Destroy(particle);
    }

    //Update Particles
    public void UpdateParticles()
    {
        foreach(GameObject particle in aliveParticles)
        {
            foreach(GameObject particle2 in aliveParticles)
            {
                if (particle == null)
                {
                    deadParticles.Add(particle);
                }
                else if (particle2 == null)
                {
                    deadParticles.Add(particle2);
                }
                else if(CollisionCheck() == true)
                {
                    //Delete both particles
                    deadParticles.Add(particle);
                    deadParticles.Add(particle2);

                }
            }
        }

        foreach(GameObject particle in deadParticles)
        {
            RemoveParticle(particle);
        }

        deadParticles.Clear();
    }

    //Collision
    public bool CollisionCheck()
    {
        //Get all bullets on screen
        bullets = GameObject.FindGameObjectsWithTag("Bullet");

        foreach (GameObject bullet in bullets)
        {
            if (Vector2.Distance(bullet.transform.position, gameObject.transform.position) < 0.2)
            {
                Debug.Log("True");
                manager.GetComponent<GameManager>().score++;
                manager.GetComponent<GameManager>().scoreText.text = manager.GetComponent<GameManager>().score.ToString();
                Destroy(gameObject);
                return true;
            }
        }

        return false;
    }
}
