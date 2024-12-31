using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossMovement : MonoBehaviour
{
    Rigidbody[] ragdollBodies;
    NavMeshAgent na;
    Animator an;
    public List<GameObject> locations = new List<GameObject>();
    int todo;
    int currentDestination=100;
    int newDestination;
    public bool dead = false;
    GameObject barrier;
    int location = 0;
    public GameObject rock;
    private Manager manager;
    public bool gonnaThrow = false;

    private bool levelEnd = false;
    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("Manager").GetComponent<Manager>();
        barrier = GameObject.FindGameObjectWithTag("Barrier");
        na = transform.root.GetComponent<NavMeshAgent>();
        an = transform.root.GetComponent<Animator>();
        ragdollBodies = transform.root.GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rb in ragdollBodies)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }
        GoToDestination();
    }
    public void CancelThrow()
    {
        CancelInvoke("ThrowRock");

    }
    private void OnTriggerEnter(Collider other)
    {
        if(!dead)
        {
            if (other.CompareTag("Destination"))
            {
                /*currentDestination = newDestination;
                Choice();*/

                location++;

                if (location >= locations.Count)
                {
                    location = 0;
                }
                gonnaThrow = true;
                an.SetTrigger("BossSwipe");
                Invoke("ThrowRock", 1f);
                Invoke("GoToDestination",2f);
            }

        }
        
    }
    void ThrowRock()
    {
        if(manager.barrierHealth > 0)
        {
            Instantiate(rock, transform.position + new Vector3(0, 4, 0), Quaternion.identity);
        }
    }
   public void Dieded()
    {
        dead = true;
    }
    public void StartWalking()
    {
        if (!dead)
        {
            an.SetTrigger("Walk");
            na.isStopped = false;
        }
            
    }
    void Choice()
    {   if(!dead)
        {
            todo = Random.Range(0, 2);
            if (todo == 0)
            {
                Debug.Log("Angry emotion");
                an.SetTrigger("Angry");
                Invoke("Choice", 3f);
            }
            else
            {
                Debug.Log("Going to another destination");
                GoToDestination();
            }
        }
        
    }
    void GoToDestination()
    {
        /*newDestination = Random.Range(0, locations.Count - 1);
        if (newDestination==currentDestination)
        {
            Choice();
        }
        else
        {
            na.SetDestination(locations[newDestination].transform.position);
            an.SetTrigger("Walk");
        }*/
        if (!dead)
        {
            na.SetDestination(locations[location].transform.position);
            an.SetTrigger("Walk");
        }
        
    }
    
    // Update is called once per frame
    void Update()
    {
        if(manager.barrierHealth > 0)
        {
            transform.root.LookAt(barrier.transform);
        }

        else
        {
            if(!levelEnd)
            {
                na.enabled = false;
                an.SetTrigger("Dance");
            }
            levelEnd = true;
        }
    }
}
