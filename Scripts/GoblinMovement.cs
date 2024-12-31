using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using MoreMountains.Feedbacks;
public class GoblinMovement : MonoBehaviour
{
    private NavMeshAgent na;
    private Animator an;
    public float runDelay = 3f;

    private GameObject barrier;
    // Start is called before the first frame update
    private void Awake()
    {
        na = GetComponent<NavMeshAgent>();
        an = GetComponent<Animator>();
    }
    public void StartLaughing()
    {
        an.SetTrigger("Laughing");
        GameObject.FindGameObjectWithTag("GoblinLaughFeedback").transform.GetComponent<MMFeedbacks>()?.PlayFeedbacks();
    }
    void Start()
    {
        /*barrier = GameObject.FindGameObjectWithTag("Barrier");*/
        if(SceneManager.GetActiveScene().name=="Scene 4")
        {
            an.SetTrigger("Shield");
        }
        //an.SetTrigger("Shield");
        /*an.SetTrigger("Talking");*/
        if(SceneManager.GetActiveScene().name=="Scene 5")
        {
            Debug.Log("Screaming now");
            an.SetTrigger("Scream");
        }
        if(SceneManager.GetActiveScene().name == "Scene 5.1")
        {
            an.SetTrigger("Scream");/*
            Invoke("Run", runDelay);*/
        }
        /*if(SceneManager.GetActiveScene().name!="Scene 5.2")
        {

            Invoke("Run", runDelay);

        }*/
        //Invoke("Run", runDelay);// making change for scene video
    }
    private void Run()
    {
        if(!transform.GetComponent<EnemyManager>().dead || !transform.GetComponent<EnemyManager>().hitByElectricity)
        {
            an.SetTrigger("Run");
            na.SetDestination(new Vector3(transform.position.x, transform.position.y, 41f/*transform.position.z - 30f*/));
            //na.SetDestination(new Vector3((barrier.transform.position.x + Random.Range(-1, 5)), transform.position.y, (barrier.transform.position.z + 0.2f)));
        }
    }
}
