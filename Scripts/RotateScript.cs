using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateScript : MonoBehaviour
{
    public float turnSpeed;
    public GameObject boss;
    Rigidbody[] ragdollBodies;
    public List<GameObject> children;
    bool dead = false;

    private void Start()
    {
        ragdollBodies = GetComponentsInChildren<Rigidbody>();
    }
    public void BossDeath()
    {
        dead = true;
        foreach(GameObject g in children)
        {
            g.GetComponent<OscillateScript>().enabled = false;
        }
        foreach(Rigidbody rb in ragdollBodies)
        {
            rb.useGravity = true;
            rb.isKinematic = false;
        }
    }
    // Update is called once per frame
    void Update()
    {   if(dead==false)
        {
            transform.position = boss.transform.position + new Vector3(0, 3, 0); ;
            transform.Rotate(Vector3.up, turnSpeed * Time.deltaTime);
        }
       
    }
}
