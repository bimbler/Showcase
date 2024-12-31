using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using MoreMountains.Feedbacks;
public class BossManager : MonoBehaviour
{
    public float health = 10;
    public GameEvent Death;
    Rigidbody[] ragdollBodies;
    NavMeshAgent na;
    Animator an;
    bool canGetHit=true;
    public GameObject spine;
    public GameObject shield;
    public GameObject directionalBlood;
    public GameObject splashBlood;
    private bool spawnedBlood = false;
    // Start is called before the first frame update
    void Start()
    {
        na = GetComponent<NavMeshAgent>();
        an = GetComponent<Animator>();
        ragdollBodies = GetComponentsInChildren<Rigidbody>();
    }
    public void HitByArrow(float x, int type)
    {   
        Debug.Log("POLAYING SOUND");
        if(canGetHit==true)
        {if(spine.GetComponent<BossMovement>().gonnaThrow==true)
            {
                spine.GetComponent<BossMovement>().CancelThrow();
                spine.GetComponent<BossMovement>().gonnaThrow = false;
            }
            GameObject.FindGameObjectWithTag("ArrowPierceFeedback").transform.GetComponent<MMFeedbacks>()?.PlayFeedbacks();
            health -= x;
            GameObject.FindGameObjectWithTag("Manager").GetComponent<Manager>().decreaseEnemyHealth(x);
            na.isStopped = true;
            canGetHit = false;
            if (health <= 0)
            {   
                Death.Raise();
                GameObject.FindGameObjectWithTag("BossDeathFeedback").transform.GetComponent<MMFeedbacks>()?.PlayFeedbacks();
                shield.GetComponent<RotateScript>().BossDeath();
            }
            else
            {   
                switch(type)
                {
                    case 1:
                        an.SetTrigger("FireBoss");
                        Invoke("DoWalk", 3f);
                        GameObject.FindGameObjectWithTag("BossHitFeedback").transform.GetComponent<MMFeedbacks>()?.PlayFeedbacks();
                        break;
                    case 2:
                        an.SetTrigger("BossShocked");
                        Invoke("DoWalk", 2f);
                        GameObject.FindGameObjectWithTag("BossHitFeedback").transform.GetComponent<MMFeedbacks>()?.PlayFeedbacks();
                        break;
                    case 3:
                        an.SetTrigger("Damn");
                        Invoke("DoWalk", 1f);
                        GameObject.FindGameObjectWithTag("BossHitFeedback").transform.GetComponent<MMFeedbacks>()?.PlayFeedbacks();
                        break;
                    default:
                        an.SetTrigger("Damn");
                        Invoke("DoWalk", 1f);
                        GameObject.FindGameObjectWithTag("BossHitFeedback").transform.GetComponent<MMFeedbacks>()?.PlayFeedbacks();
                        break;

                }
                
            }
            Invoke("ResetHit", 1f);
        }
    }

    public void SpawnBlood(Vector3 bloodLoc)
    {
        if(!spawnedBlood)
        {
            Instantiate(splashBlood, bloodLoc, Quaternion.identity);
            Instantiate(directionalBlood, bloodLoc, Quaternion.identity);
        }
        spawnedBlood = true;
    }

    void ResetHit()
    {
        canGetHit = true;
        spawnedBlood = false;
    }
    void DoWalk()
    {
        spine.GetComponent<BossMovement>().StartWalking();
    }
    public void ActivateRagdoll()
    {
        na.enabled = false;
        an.enabled = false;
        foreach (Rigidbody rb in ragdollBodies)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }
    }
}
