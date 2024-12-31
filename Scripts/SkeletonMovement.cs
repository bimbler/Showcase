using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class SkeletonMovement : MonoBehaviour
{
    public float walkDelay = 3f;

    private NavMeshAgent na;
    private Animator an;
    private GameObject barrier;
    // Start is called before the first frame update
    private void Awake()
    {
        na = GetComponent<NavMeshAgent>();
        an = GetComponent<Animator>();
    }
    void Start()
    {
        /*barrier = GameObject.FindGameObjectWithTag("Barrier");*/
        an.SetTrigger("Talking");
        /*InvokeMovement(); */ //making changes for scene videos
    }

    public void InvokeMovement()
    {
        Invoke("Walk", walkDelay);
    }

    private void Walk()
    {
        if (!transform.GetComponent<EnemyManager>().dead && !transform.GetComponent<EnemyManager>().hitByElectricity)
        {
            na.SetDestination(new Vector3((barrier.transform.position.x + Random.Range(-1, 5)), transform.position.y, (barrier.transform.position.z + 0.3f)));
            /*Debug.Log("Destination is " + na.destination);*/
            an.SetTrigger("Walk");
        }
    }
}
