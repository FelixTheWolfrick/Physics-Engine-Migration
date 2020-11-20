using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
//using System.Numerics;
using UnityEngine;

public class Particle2DLink : MonoBehaviour
{
    //Variables
    public GameObject mObj1;
    public GameObject mObj2;

    public Particle2DContact particleContactOne;
    public Particle2DContact particleContactTwo;

    public float maxLength = 10.0f;

    //Initialize Variables (Constructor)
    public void Initialize(GameObject object1, GameObject object2)
    {
        mObj1 = object1;
        mObj2 = object2;
    }

    //Create Contacts
    public virtual void CreateContacts(List<Particle2DContact> contacts)
    {

    }

    //Create Particle Link
    public Particle2DLink CreateLink(GameObject object1, GameObject object2, float maxLength)
    {
        ParticleRod particleRod = object1.AddComponent<ParticleRod>();
        particleRod.Initialize(object1, object2, maxLength);
        return object1.GetComponent<ParticleRod>();
    }
    
    //Get Length
    public float GetLength(Vector2 tempVector)
    {
        float length = GetLengthSquared(tempVector);
        return Mathf.Sqrt(length);
    }

    //Get Length Squared
    public float GetLengthSquared(Vector2 tempVector)
    {
        float lengthSquared = ((tempVector.x * tempVector.x) + (tempVector.y * tempVector.y));
        return Mathf.Sqrt(lengthSquared);
    }

    //Get Distance
    public float GetDistance(Vector2 positionOne, Vector2 positionTwo)
    {
        Vector2 distance = positionOne - positionTwo;
        return GetLength(distance);
    }
}

public class ParticleCable : Particle2DLink
{
    //Variables
    float mMaxLength;
    float mRestitution = 0.5f;

    //Initialize Variables (Constructor)
    public void Initialize(float maxLength, float restitution)
    {
        mMaxLength = maxLength;
        mRestitution = restitution;
    }

    //Create Contacts
    public override void CreateContacts(List<Particle2DContact> contacts)
    {
        //base.CreateContacts(contacts);

        //Variables
        Vector2 normal;
        float length = GetLength(gameObject.transform.position);

        if(length < mMaxLength)
        {
            return;
        }

        normal = mObj2.transform.position - gameObject.transform.position;
        normal.Normalize();

        float penetration = length - mMaxLength;

        Particle2DContact newParticleContact = new Particle2DContact();
        newParticleContact.Initialize(mObj1, mObj2, mRestitution, normal, penetration, Vector2.zero, Vector2.zero);
        contacts.Add(newParticleContact);
    }
}

public class ParticleRod : Particle2DLink
{
    //Variables
    float mLength;

    //Initialize Variables (Constructor)
    public void Initialize(GameObject obj1, GameObject obj2, float length)
    {
        mObj1 = obj1;
        mObj2 = obj2;
        mLength = length;
    }

    //Create Contacts
    public override void CreateContacts(List<Particle2DContact> contacts)
    {
        //base.CreateContacts(contacts);

        //Check to Make Sure They Exist
        if(mObj1 == null || mObj2 == null)
        {
            Destroy(gameObject);
            return;
        }

        //Variables
        float penetration;

        Vector2 normal;
        normal = mObj2.transform.position - mObj1.transform.position;
        normal.Normalize();

        float length = GetDistance(mObj1.transform.position, mObj2.transform.position);

        if(length == mLength)
        {
            return;
        }

        if(length > mLength)
        {
            //Particle2DContact newParticleContact = gameObject.AddComponent<Particle2DContact>();
            //newParticleContact.Initialize(mObj1, mObj2, 0.0f, normal, mLength - length, Vector2.zero, Vector2.zero);
            //contacts.Add(newParticleContact);

            penetration = length - mLength;
        }
        else
        {
            //Particle2DContact newParticleContact = gameObject.AddComponent<Particle2DContact>();
            //newParticleContact.Initialize(mObj1, mObj2, 0.0f, normal * -1, length - mLength, Vector2.zero, Vector2.zero);
            //contacts.Add(newParticleContact);

            normal *= -1;
            penetration = mLength - length;
        }

        penetration *= 0.001f;
        mObj1.AddComponent<Particle2DContact>();
        Particle2DContact newParticleContact = mObj1.GetComponent<Particle2DContact>();
        newParticleContact.Initialize(mObj1, mObj2, 0.0f, normal, penetration, Vector2.zero, Vector2.zero);
        contacts.Add(newParticleContact);
    }
}