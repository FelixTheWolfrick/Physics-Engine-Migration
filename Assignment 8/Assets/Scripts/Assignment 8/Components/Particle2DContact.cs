using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using UnityEngine;

public class Particle2DContact : MonoBehaviour
{
    //Variables
    public GameObject mObj1;
    public GameObject mObj2;
    public Particle2D particleOne;
    public Particle2D particleTwo;
    public float mRestitutionCoefficient = 0.0f;
    public Vector2 mContactNormal = new Vector2(0.0f, 0.0f);
    public float mPenetration = 0.0f;
    public Vector2 mMove1 = new Vector2(0.0f, 0.0f);
    public Vector2 mMove2 = new Vector2(0.0f, 0.0f);

    //Initialize Variables (Constructor)
    public void Initialize(GameObject object1, GameObject object2, float restitutionCoefficient, Vector2 contactNormal, float penetration, Vector2 move1, Vector2 move2)
    {
        mObj1 = object1;
        mObj2 = object2;
        mRestitutionCoefficient = restitutionCoefficient;
        mContactNormal = contactNormal;
        mPenetration = penetration;
        mMove1 = move1;
        mMove2 = move2;

        particleOne = mObj1.GetComponent<Particle2D>();
        if(particleTwo)
        {
            particleTwo = mObj2.GetComponent<Particle2D>();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Resolve
    public void Resolve()
    {
        ResolveVelocity();
        ResolveInterpenetration();
    }

    //Calculate Seperating Velocity
    public float CalculateSeperatingVelocity()
    {
        //Variables
        Vector2 relativeVel = particleOne.velocity;

        if(mObj2)
        {
            relativeVel -= particleTwo.velocity;
        }

        //Calculate
        return DotProduct(relativeVel, mContactNormal);
    }

    //Seperating Velocity
    public float SeperatingVelocity()
    {
        //Variables
        Vector2 relativeVel = particleOne.velocity;

        if (particleTwo)
        {
            relativeVel -= particleTwo.acceleration;
        }

        //Calculate
        return DotProduct(relativeVel, transform.forward);
    }

    //Dot Product (I think unity has something called xxx.Dot? not sure though)
    public float DotProduct(Vector2 vectorOne, Vector2 vectorTwo)
    {
        float calculations = ((vectorOne.x * vectorTwo.x) + (vectorOne.y * vectorTwo.y)); 
        return calculations;
    }

    //Resolve Velocity
    public void ResolveVelocity()
    {
        float separatingVel = CalculateSeperatingVelocity();
        
        //If Already Separating
        if(separatingVel > 0.0f)
        {
            return;
        }

        float newSepVel = -separatingVel * mRestitutionCoefficient;

        Vector2 velFromAcc = particleOne.acceleration;
        if(mObj2)
        {
            velFromAcc -= particleTwo.acceleration;
        }
        float accCausedSepVelocity = DotProduct(velFromAcc, transform.forward) * Time.deltaTime;

        if(accCausedSepVelocity < 0.0f)
        {
            newSepVel += mRestitutionCoefficient * accCausedSepVelocity;

            if(newSepVel < 0.0f)
            {
                newSepVel = 0.0f;
            }
        }

        float dataVel = newSepVel - separatingVel;

        float totalInverseMass = (float)(1.0 / particleOne.mass);
        
        if (mObj2)
        {
            totalInverseMass += (float)(1.0 / particleTwo.mass);
        }

        if(totalInverseMass <= 0)
        {
            return;
        }

        float impulse = dataVel / totalInverseMass;
        Vector2 impulsePerMass = mContactNormal * impulse;

        Vector2 newVelocity = (particleOne.velocity + impulsePerMass) * (float)(1.0 / particleOne.mass);
        //mObj1.GetComponent<Particle2D>().velocity = newVelocity;

        //if(mObj2)
        //{
        //    Vector2 newVelocity2 = mObj2.GetComponent<Particle2D>().velocity + impulsePerMass * (float)(1.0 / mObj2.GetComponent<Particle2D>().mass);
        //    mObj2.GetComponent<Particle2D>().velocity = newVelocity2;
        //}
    }

    //Resolve Interpenetration
    public void ResolveInterpenetration()
    {
        if(mPenetration <= 0.0f)
        {
            return;
        }

        float totalInverseMass = (float)(1.0 / particleOne.mass);

        if(mObj2)
        {
            totalInverseMass += (float)(1.0 / particleTwo.mass);
        }

        if(totalInverseMass <= 0)
        {
            return;
        }

        Vector2 movePerIMass = mContactNormal * (mPenetration / totalInverseMass);
        mMove1 =  movePerIMass * -(float)(1.0 / particleOne.mass);

        if(mObj2)
        {
            mMove2 = movePerIMass * (float)(1.0 / particleTwo.mass);
        }
        else
        {
            mMove2 = new Vector2(0.0f, 0.0f);
        }

        Vector2 newPosition = (Vector2)mObj1.transform.position + mMove1;
        mObj1.transform.position = newPosition;

        if(mObj2)
        {
            Vector2 newPosition2 = (Vector2)mObj2.transform.position + mMove2;
            mObj2.transform.position = newPosition2;
        }
    }
}
