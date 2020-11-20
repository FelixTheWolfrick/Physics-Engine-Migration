using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ForceManager : MonoBehaviour
{
    //Variables
    private static ForceManager instance;
    List<ForceGenerator2D> forceGeneratorsAlive = new List<ForceGenerator2D>();
    List<ForceGenerator2D> forceGeneratorsDead = new List<ForceGenerator2D>();
    ForceGenerator2D newForceGenerator;
    public GameObject waterSprite;

    public static ForceManager Instance { get { return instance; } }

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
        //CreatePointForceGenerator(new Vector2(0, 0), 1);
        //CreateBouyancyForceGenerator(gameObject, (waterSprite.transform.localScale.y) / 2.0f, 75.0f, 5.0f, -(waterSprite.transform.localScale.y) / 2.0f);
    }

    // Update is called once per frame
    void Update()
    {
        Particle2D[] allParticles = (Particle2D[])GameObject.FindObjectsOfType(typeof(Particle2D));

        foreach (ForceGenerator2D generator in forceGeneratorsAlive)
        {
            if (generator == null)
            {
                forceGeneratorsDead.Add(generator);
            }
            else
            {
                foreach (Particle2D particle in allParticles)
                {
                    if (particle.gameObject == null)
                    {
                        return;
                    }

                    if (!particle.ignoreForces)
                    {
                        generator.UpdateForce(particle.gameObject);
                    }
                }
            }
        }

        foreach(ForceGenerator2D generator in forceGeneratorsDead)
        {
            RemoveForceGenerator(generator);
        }
        forceGeneratorsDead.Clear();
    }

    //Add Force Generator
    public void AddForceGenerator(ForceGenerator2D forceGenerator)
    {
        forceGeneratorsAlive.Add(forceGenerator);
    }

    //Remove Force Generator
    void RemoveForceGenerator(ForceGenerator2D forceGenerator)
    {
        forceGeneratorsAlive.Remove(forceGenerator);
    }

    //Create Point Force Generator
    public ForceGenerator2D CreatePointForceGenerator(Vector2 point, float magnitude)
    {
        GameObject forceGenerator = new GameObject("PointForceGenerator");
        PointForceGenerator newGenerator = forceGenerator.AddComponent<PointForceGenerator>();
        newGenerator.Initialize(point, magnitude);
        AddForceGenerator(newGenerator);
        return forceGenerator.GetComponent<ForceGenerator2D>();
    }

    //Create Spring Force Generator
    public ForceGenerator2D CreateSpringForceGenerator(GameObject gameObject1, GameObject gameObject2, float springConstant, float restLength)
    {
        //GameObject forceGenerator = new GameObject("SpringForceGenerator");
        SpringForceGenerator newGenerator = gameObject1.AddComponent<SpringForceGenerator>();
        newGenerator.Initialize(gameObject1, gameObject2, springConstant, restLength);
        AddForceGenerator(newGenerator);
        return gameObject1.GetComponent<ForceGenerator2D>();
    }

    //Create Bouyancy Force Generator
    public ForceGenerator2D CreateBouyancyForceGenerator(GameObject gameObject1, float bouyancy, float volume, float density, float depth)
    {
        //GameObject forceGenerator = new GameObject("BouyancyForceGenerator");
        BouyancyForceGenerator newGenerator = gameObject.AddComponent<BouyancyForceGenerator>();
        newGenerator.Initialize(gameObject1, bouyancy, volume, density, depth);
        AddForceGenerator(newGenerator);
        return gameObject.GetComponent<ForceGenerator2D>();
    }

    //Create Bungie Force Generator
    public ForceGenerator2D CreateBungieForceGenerator(GameObject gameObject1, Vector2 point, float springConstant, float restLength)
    {
        GameObject forceGenerator = new GameObject("BungieForceGenerator");
        BungieForceGenerator newGenerator = forceGenerator.AddComponent<BungieForceGenerator>();
        newGenerator.Initialize(gameObject1, point, springConstant, restLength);
        AddForceGenerator(newGenerator);
        return forceGenerator.GetComponent<ForceGenerator2D>();
    }
}
