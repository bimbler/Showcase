using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonTest : MonoBehaviour
{
    Rigidbody[] ragdollBodies;
    Animator an;
    private void Awake()
    {
        an = GetComponent<Animator>();
        ragdollBodies = GetComponentsInChildren<Rigidbody>();
        foreach(Rigidbody rb in ragdollBodies)
        {
            rb.useGravity = false;
            rb.isKinematic = true;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        /*Invoke("boom", 2f);*/
    }

    public void boom(Transform g, Vector3 a)
    {
        an.enabled = false;
        foreach (Rigidbody rb in ragdollBodies)
        {
            rb.useGravity = true;
            rb.isKinematic = false;
            
        }
        Debug.Log(a * 50);
        g.GetComponent<Rigidbody>().AddForce(a * 50, ForceMode.Impulse);
        /*foreach(Rigidbody rb in ragdollBodies)
        {
            rb.AddExplosionForce(10000f, rb.position, 1f);
        }*/
        Invoke("stop", 2f);
    }
    public void stop()
    {
        foreach (Rigidbody rb in ragdollBodies)
        {
            rb.useGravity = false;
            rb.isKinematic = true;

        }
    }
}
