using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class NinjaMovement : MonoBehaviour
{
    public List<GameObject> points = new List<GameObject>();
    public NavMeshData nextpoint;

    public int counter = 1;
    public int currentPlace;
    public GameObject shuriken;

    private Animator an;
    private NavMeshAgent na;
    private GameObject barrier;
    // Start is called before the first frame update
    void Start()
    {
        barrier = GameObject.FindGameObjectWithTag("Barrier");
        na = GetComponent<NavMeshAgent>();
        /*na.autoTraverseOffMeshLink = false;*/
        an = GetComponent<Animator>();
        currentPlace = 0;
        Invoke("StartMoving", 1f);
        
    }
    void StartMoving()
    {   
        if(!GetComponent<EnemyManager>().dead)
        {
            counter = Random.Range(0, 4);
            if (counter == currentPlace)
            {
                Invoke("ThrowShuriken", 3.9f);
            }
            else
            {
                na.SetDestination(points[counter].transform.position);
                an.SetTrigger("Jump");
                currentPlace = counter;
                Invoke("ThrowShuriken", 3.9f);  // 1.3 original. changed for slow jump speed

            }
        }
    }
    void ThrowShuriken()
    {
        /*Debug.Log("reached here!!!!");*/
        //if condition so that shuriken is not thrown when shocking animation happens
        if (transform.root.GetComponent<EnemyManager>().dead == false) //original hitByElectricity
        {
            an.SetTrigger("Throw");
            Invoke("ThrowBomb", 0.3f);
            Invoke("StartMoving", 1.5f);
        }
    }
    void ThrowBomb()
    {   
        if(transform.root.GetComponent<EnemyManager>().dead == false && GameObject.FindGameObjectWithTag("Manager").GetComponent<Manager>().barrierHealth > 0)
        {
            GameObject.Instantiate(shuriken, transform.position + new Vector3(-0.7f, 1, 1), Quaternion.identity);
        }
        
    }
    // Update is called once per frame
    void Update()
    {
        if(!transform.GetComponent<EnemyManager>().dead)
        {
            if(barrier != null)
            {
                transform.LookAt(barrier.transform);
            }
        }
    }
}
