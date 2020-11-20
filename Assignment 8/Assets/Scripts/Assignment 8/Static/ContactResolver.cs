using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactResolver : MonoBehaviour
{
    //Variables
    private static ContactResolver instance;
    public static ContactResolver Instance { get { return instance; } }

    public List<Particle2DContact> mContacts = new List<Particle2DContact>();
    public List<Particle2DLink> mParticleLinks = new List<Particle2DLink>();

    int mIterations;
    int mIterationsUsed = 0;

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
        GameObject obj1 = GameObject.Find("Rod1");
        GameObject obj2 = GameObject.Find("Rod2");

        GameObject particleLinkObj = new GameObject("Link" + obj1.name + " " + obj2.name);
        ParticleRod newParticleRod = particleLinkObj.AddComponent<ParticleRod>();
        newParticleRod.Initialize(obj1, obj2, 2);
        mParticleLinks.Add(newParticleRod);
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Particle2DLink particleLink in mParticleLinks)
        {
            particleLink.CreateContacts(mContacts);
        }
        ResolveContacts(mContacts, 10);
        mContacts.Clear();
    }

    //Resolve Contacts
    public void ResolveContacts(List<Particle2DContact> contacts, int iterations)
    {
        mIterationsUsed = 0;

        while(mIterationsUsed > iterations)
        {
            //Variables
            float max = float.MaxValue;
            int numContacts = contacts.Count;
            int maxIndex = numContacts - 1;
            float sepVel;

            for(int i = 0; i < numContacts; i++)
            {
                sepVel = contacts[i].CalculateSeperatingVelocity();

                if(sepVel < max && (sepVel < 0.0f || contacts[i].mPenetration > 0.0f))
                {
                    max = sepVel;
                    maxIndex = 1;
                }
            }

            if(maxIndex == numContacts)
            {
                break;
            }

            contacts[maxIndex].Resolve();

            for(int i = 0; i < numContacts; i++)
            {
                if(contacts[i].mObj1 == contacts[maxIndex].mObj1)
                {
                    contacts[i].mPenetration -= Vector2.Dot(contacts[maxIndex].mMove1, contacts[i].mContactNormal);
                }
                else if(contacts[i].mObj1 == contacts[i].mObj2)
                {
                    contacts[i].mPenetration -= Vector2.Dot(contacts[maxIndex].mMove2, contacts[i].mContactNormal);
                }

                if(contacts[i].mObj2)
                {
                    if(contacts[i].mObj2 == contacts[maxIndex].mObj1)
                    {
                        contacts[i].mPenetration += Vector2.Dot(contacts[maxIndex].mMove1, contacts[i].mContactNormal);
                    }
                    else if(contacts[i].mObj2 == contacts[i].mObj2)
                    {
                        contacts[i].mPenetration -= Vector2.Dot(contacts[maxIndex].mMove2, contacts[i].mContactNormal);
                    }
                }
            }
            mIterationsUsed++;
        }
    }
}
