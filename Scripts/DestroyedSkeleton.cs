using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyedSkeleton : MonoBehaviour
{
    Rigidbody[] ragdollBodies;
    // Start is called before the first frame update
    void Start()
    {
        ragdollBodies = GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rb in ragdollBodies)
        {
            rb.AddExplosionForce(10f, transform.position, 5f, 1.4f, ForceMode.Impulse);
        }

    }
}
