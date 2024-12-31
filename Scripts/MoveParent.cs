using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveParent : MonoBehaviour
{
    public float movSpeed;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        var locVel = transform.InverseTransformDirection(rb.velocity);
        locVel.y = movSpeed;
        rb.velocity = transform.TransformDirection(locVel);
    }
    
}
