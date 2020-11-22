using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    //Variables
    private static Particle2D instance;
    public static Particle2D Instance { get { return instance; } }

    public List<Particle2D> aliveParticles = new List<Particle2D>();
    List<Particle2D> deadParticles = new List<Particle2D>();
    //Controls manager;
    public GameObject manager;
    //Target targetHit;
    Particle2D[] particleList;

    // Start is called before the first frame update
    void Start()
    {
        //AddParticle();
    }

    // Update is called once per frame
    void Update()
    {
        AddParticle();
        UpdateParticles();
    }

    //Add Particle
    public void AddParticle()
    {
        particleList = (Particle2D[])(GameObject.FindObjectsOfType(typeof(Particle2D)));

        foreach (Particle2D particleAdd in particleList)
        {
            aliveParticles.Add(particleAdd);
        }
    }

    //Remove Particle
    public void RemoveParticle(Particle2D particle)
    {
        deadParticles.Remove(particle);
        Destroy(particle.gameObject);
    }

    //Update Particles
    public void UpdateParticles()
    {
        foreach(Particle2D particleOne in aliveParticles)
        {
            foreach(Particle2D particleTwo in aliveParticles)
            {
                //if (CheckCollision.CollisionCheck(particleOne, particleTwo))
                //{
                //    if (particleOne != particleTwo) //Make sure it doesn't detect itself
                //    {
                //        deadParticles.Add(particleOne);
                //        deadParticles.Add(particleTwo);
                //    }
                //}
            }
        }

        foreach(Particle2D particle in deadParticles)
        {
            RemoveParticle(particle);
        }

        deadParticles.Clear();
    }

    //Collision
    //public bool CollisionCheck()
    //{
    //    //Get all bullets on screen
    //    bullets = (Particle2D[])(GameObject.FindObjectsOfType(typeof(Particle2D)));

    //    foreach (Particle2D bullet in bullets)
    //    {
    //        if (Vector2.Distance(bullet.transform.position, gameObject.transform.position) < 0.2)
    //        {
    //            Debug.Log("True");
    //            manager.GetComponent<GameManager>().score++;
    //            manager.GetComponent<GameManager>().scoreText.text = manager.GetComponent<GameManager>().score.ToString();
    //            Destroy(gameObject);
    //            return true;
    //        }
    //    }

    //    return false;
    //}
}
