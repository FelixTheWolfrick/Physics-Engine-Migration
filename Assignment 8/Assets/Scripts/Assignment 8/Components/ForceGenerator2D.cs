using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime;
using System;
//using System.Numerics;

public class ForceGenerator2D : MonoBehaviour
{
    //Variables
    public bool mShouldEffectAll;

    //Functions
    public virtual void UpdateForce(GameObject gameObject1)
    {

    }

    public void addForce(GameObject gameObject1, Vector2 force)
    {
        gameObject1.GetComponent<Particle2D>().accumulatedForces += force;
    }
}

//Point Force Generator
public class PointForceGenerator : ForceGenerator2D
{
    //Vairables
    private Vector2 mPoint;
    private float mMagnitude;

    //Initialize Variables (Constructor)
    public void Initialize(Vector2 point, float magnitude)
    {
        mPoint = point;
        mMagnitude = magnitude;
        mShouldEffectAll = true;
    }

    //Update Force
    public override void UpdateForce(GameObject gameObject1)
    {
        //base.UpdateForce(gameObject, dt);

        //Variables
        Vector2 diff = mPoint - (Vector2)gameObject1.transform.position;
        float range = 10;
        float rangeSQ = range * range;
        float dist = Vector2.Distance(mPoint, gameObject1.transform.position);
        float distSQ = (float)Math.Sqrt(dist);

        //Calculate 
        if(distSQ < rangeSQ)
        {
            dist = Vector2.Distance(mPoint, gameObject1.transform.position);
            float proportionAway = dist / range;
            proportionAway = 1 - proportionAway;
            diff.Normalize();

            //gameObject1.GetComponent<Particle2D>().accumulatedForces += (diff * (mMagnitude * proportionAway));

            addForce(gameObject1, (diff * (mMagnitude * proportionAway)) * Time.deltaTime);
        }
    }
}

//Spring Force Generator
public class SpringForceGenerator : ForceGenerator2D
{
    //Variables
    private GameObject mGameObject1;
    private GameObject mGameObject2;
    private float mSpringConstant;
    private float mRestLength;

    //Initialize Variables (Constructor)
    public void Initialize(GameObject gameObject1, GameObject gameObject2, float springConstant, float restLength)
    {
        mGameObject1 = gameObject1;
        mGameObject2 = gameObject2;
        mSpringConstant = springConstant;
        mRestLength = restLength;
        mShouldEffectAll = false;
    }

    //Update Force
    public override void UpdateForce(GameObject gameObject1)
    {
        //base.UpdateForce(gameObject1, dt);

        //Make Sure Objects Exist
        if(mGameObject1 == null || mGameObject2 == null)
        {
            Destroy(gameObject);
            return;
        }

        //Get Position of Objects
        Vector2 mGameObject1Pos = mGameObject1.transform.position;
        Vector2 mGameObject2Pos = mGameObject2.transform.position;

        //Calculate
        Vector2 diff = mGameObject1Pos - mGameObject2Pos;
        float dist = Vector2.Distance(mGameObject1Pos, mGameObject2Pos);

        float magnitude = dist - mRestLength;
        magnitude *= mSpringConstant;

        diff.Normalize();
        diff *= -magnitude;

        Vector2 diffOpposite = new Vector2(-diff.x + 2, -diff.y + 2);

        addForce(mGameObject1, diff);
        addForce(mGameObject2, diffOpposite); //Diff in opposite direction FIX, I LITERALLY LEFT MYSELF A COMMENT HOW DID I STILL FORGET TO DO THIS
    }
}

//Bouyancy Force Generator
public class BouyancyForceGenerator : ForceGenerator2D
{
    //Variables
    private GameObject mGameObject1;
    private float mBouyancy;
    private float mVolume, mDensity, mMaxDepth;

    //Initialize Variables (Constructor)
    public void Initialize(GameObject gameObject1, float bouyancy, float volume, float density, float maxDepth)
    {
        mGameObject1 = gameObject1;
        mBouyancy = bouyancy;
        mVolume = volume;
        mDensity = density;
        mMaxDepth = maxDepth;
        mShouldEffectAll = false;
    }

    //Update Force
    public override void UpdateForce(GameObject gameObject1)
    {
        //base.UpdateForce(gameObject1, dt);

        //Make Sure Objects Exist
        if (mGameObject1 == null)
        {
            return;
        }

        //Variables
        Vector2 force = new Vector2(0, 0);
        float depth = (mGameObject1.transform.position.y);

        //Update Force
        if(depth >= mBouyancy)
        {
            gameObject1.GetComponent<Particle2D>().dampingCost = 0.99f;
            return;
        }

        gameObject1.GetComponent<Particle2D>().dampingCost = 0.6f;

        if (depth >= (mBouyancy + mMaxDepth)) //Out of Water
        {
            force.y = (-1 * (mDensity * mVolume));
        }
        else if(depth <= (mBouyancy - mMaxDepth)) //Max Depth
        {
            force.y = (mDensity * mVolume);
        }
        else //In Water
        {
            force.y = mDensity * mVolume * (depth - mMaxDepth - mBouyancy) / 2 * mMaxDepth;
        }

        addForce(mGameObject1, force * 0.1f);
    }
}

//Bungie Force Generator
public class BungieForceGenerator : ForceGenerator2D
{
    //Variables
    private GameObject mGameObject1;
    private Vector2 mPoint;
    private float mSpringConstant, mRestLength;

    //Initialize Variables (Constructor)
    public void Initialize(GameObject gameObject1, Vector2 point, float springConstant, float restLength)
    {
        mGameObject1 = gameObject1;
        mPoint = point;
        mSpringConstant = springConstant;
        mRestLength = restLength;
        mShouldEffectAll = false;
    }

    //Update Force
    public override void UpdateForce(GameObject gameObject1)
    {
        //base.UpdateForce(gameObject1, dt);

        //Make Sure Objects Exist
        if (mGameObject1 == null)
        {
            return;
        }

        //Variables
        Vector2 pos1 = mGameObject1.transform.position;
        Vector2 pos2 = mPoint;

        Vector2 diff = pos1 - pos2;
        float dist = Vector2.Distance(pos1, pos2);

        float magnitude = dist - mRestLength;

        //Calculate
        if(magnitude < 0.0f)
        {
            return;
        }

        magnitude *= mSpringConstant;

        diff.Normalize();
        diff *= -magnitude;

        addForce(mGameObject1, diff);
    }
}