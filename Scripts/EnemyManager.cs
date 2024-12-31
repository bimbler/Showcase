using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using UnityEngine.AI;
public class EnemyManager : MonoBehaviour
{
    Rigidbody[] ragdollBodies;
    Animator an;
    public int EnemyType = 1;
    bool hit = false;
    public GameObject body;
    public Material burnMaterial;
    public GameObject ElectricityEnemy;
    public bool hitByElectricity = false;
    public bool ricochet = false;
    public bool hitByFire = false;
    public bool hitByBomb = false;
    public bool dead = false;
    bool attackBarrier = true;
    public float damageToBarrier = 20f;
    public float damageToBarrierTime = 1f;
    public List<GameObject> enemyList = new List<GameObject>();
    public GameObject electricityTrail;
    public GameObject fireBody;
    public GameObject destroyedSkel;
    public List<GameObject> bodyParts= new List<GameObject>();
    public NavMeshAgent na;
    public GameObject directionalBlood;
    public GameObject splashBlood;
    private bool spawnedBlood = false;

    //Getting Navmesh, Animator, and ragdoll components (Rigidbodies)
    private void Awake()
    {
        na = GetComponent<NavMeshAgent>();
        an = GetComponent<Animator>();
        ragdollBodies = GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rb in ragdollBodies)
        {
            rb.useGravity = false;
            rb.isKinematic = true;
        }
    }

    //Start is called before the first frame update
    void Start()
    {

    }

    //Skeleton death (called during Explosion and Electricity)
    public void ExplodeSkeleton(Vector3 arrowPos)  
    {
        GameObject SkeletonMini = Instantiate(destroyedSkel, transform.position, transform.rotation); //new skeleton code
        Rigidbody[] ragdolls = SkeletonMini.GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rb in ragdolls)
        {
            rb.AddExplosionForce(100f, transform.position, 3f, 2f, ForceMode.Impulse);
        }
        GameObject.Destroy(this.gameObject);
    }

    //Returns all closest enemies in a radius
    public List<GameObject> FindClosestEnemies()
    {
        
        GameObject[] gos;
        List<GameObject> nearestEnemies = new List<GameObject>();
        gos = GameObject.FindGameObjectsWithTag("EnemyParent");
        foreach (GameObject go in gos)
        {
            Debug.Log(go.transform.name);
            if (go.GetComponent<EnemyManager>().hitByElectricity == false && go.GetComponent<EnemyManager>().dead==false)
            {
                enemyList.Add(go);
            }
        }
        
        Vector3 position = transform.position;
        foreach (GameObject go in enemyList)
        {
            if (Mathf.Abs(Vector3.Distance(go.transform.position, transform.position)) < 10)
            {
                nearestEnemies.Add(go);
            }
        }
        return nearestEnemies;
    }

    //Enable ragdoll
    public void EnableRagdoll()
    {
        an.enabled = false;
        foreach (Rigidbody rb in ragdollBodies)
        {
            rb.useGravity = true;
            rb.isKinematic = false;
        }
    }

    //Default arrow death
    public void Death(Transform g, Vector3 a)
    {
        if(!dead)
        {
            if (hit == false)
            {

                #region EnemyType Description
                //Type 1 = Skeleton
                //Type 2 = Default
                //Type 3 = Ninja
                #endregion
                switch (EnemyType)
                {
                    case 1:
                        Death2();
                        GameObject SkeletonMini = Instantiate(destroyedSkel, transform.position, transform.rotation);//new skeleton code
                        Rigidbody[] ragdolls = SkeletonMini.GetComponentsInChildren<Rigidbody>();
                        foreach (Rigidbody rb in ragdolls)
                        {
                            rb.AddExplosionForce(5f, transform.position, 1f, 1f, ForceMode.Impulse);
                        }
                        GameObject.Destroy(this.gameObject);// new skeleton code
                        break;

                    case 2:
                        Death2();
                        EnableRagdoll();
                        g.GetComponent<Rigidbody>().AddForce(a * 1000/*, ForceMode.Impulse*/);
                        break;

                    case 3:
                        Death2();
                        EnableRagdoll();
                        GetComponent<AgentLinkMover>().enabled = false;
                        g.GetComponent<Rigidbody>().AddForce(a * 250, ForceMode.Impulse);
                        break;

                    default:
                        break;
                }
                hit = true;
                /*Invoke("ResetHit", 1f);*/
            }
        }       
    }

    public void SpawnBlood(Vector3 bloodLoc)
    {
        if(!spawnedBlood)
        {
            Instantiate(splashBlood, bloodLoc, Quaternion.identity);
            Instantiate(directionalBlood, bloodLoc, Quaternion.identity);
            spawnedBlood = true;
        }
    }

    public void Death2()
    {
        
        na.Warp(transform.position);
        na.enabled = false;
        dead = true;
        GameObject.FindGameObjectWithTag("Manager").GetComponent<Manager>().decreaseEnemyHealth(1f);
        GameObject.FindGameObjectWithTag("CPIManager").GetComponent<CPIManager>().DecreaseEnemyCount();

        //add enemy death sounds
        switch(EnemyType)
        {
            case 1:
                GameObject.FindGameObjectWithTag("SkeletonDeathFeedback").transform.GetComponent<MMFeedbacks>()?.PlayFeedbacks();
                break;

            case 2:
                GameObject.FindGameObjectWithTag("HumanoidDeathFeedback").transform.GetComponent<MMFeedbacks>()?.PlayFeedbacks();
                break;

            case 3:
                GameObject.FindGameObjectWithTag("HumanoidDeathFeedback").transform.GetComponent<MMFeedbacks>()?.PlayFeedbacks();
                break;

            default:
                break;
        }
    }

    //Electricity and ricochet
    public void ElectricityStart()
    {
        
        hitByElectricity = true;
        na.enabled = false;
        if (EnemyType == 3)
        {
            dead = true;
            Debug.Log("electricity");
            GetComponent<AgentLinkMover>().enabled = false;
            /*foreach (Rigidbody rb in ragdollBodies)
            {
                Debug.Log("RAGDOLL!");
                rb.useGravity = false;
                rb.isKinematic = true;
            }*/
        }

        Invoke("ElectricityDeath", 2f);
        an.SetTrigger("Shocked");
        GameObject shock = Instantiate(ElectricityEnemy, new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), Quaternion.identity);
        shock.transform.parent = this.transform;
        Destroy(shock, 2f);

        //richochet part
        if (ricochet == false)
        {
            ricochet = true;
            List<GameObject> closestEnemies = FindClosestEnemies();
            foreach(GameObject g in closestEnemies)
            {
                g.transform.GetComponent<EnemyManager>().hitByElectricity = true;
            }

            foreach (GameObject g in closestEnemies)
            {
                GameObject elec = Instantiate(electricityTrail, (g.transform.position + transform.position) / 2, Quaternion.identity);
                GameObject.FindGameObjectWithTag("LightningRicochetFeedback").transform.GetComponent<MMFeedbacks>()?.PlayFeedbacks();
                elec.transform.localScale *= Mathf.Abs(Vector3.Distance(g.transform.position, transform.position));
                elec.transform.LookAt(g.transform.position);
                elec.transform.position = new Vector3(elec.transform.position.x, elec.transform.position.y + 1, elec.transform.position.z);
                Destroy(elec, 1f);
                g.transform.GetComponent<EnemyManager>().callElec();
            }
        }
    }

    //Electricity delay for Ricochet
    void callElec() 
    {
        Invoke("ElectricityStart", 0.5f);
    }
 
    private void ElectricityDeath()
    {
        Death2();
        EnableRagdoll();
        if (EnemyType == 1)
        {
            ExplodeSkeleton(transform.position);
        }

        //For Ninja
        if (EnemyType == 3)
        {
            GetComponent<AgentLinkMover>().enabled = false;
        }
    }

    public void FireStart()  //fire start
    {   
        if(hitByFire == false)
        {
            /*GameObject.FindGameObjectWithTag("DeathFeedback").transform.GetComponent<MMFeedbacks>()?.PlayFeedbacks();*/
            Instantiate(fireBody, transform.position+new Vector3(0,0.2f,0),Quaternion.identity);
            Invoke("FireDeath", 2f);
            an.SetTrigger("Fire");
            na.isStopped = true;
            hitByFire = true;
        } 
    }

    private void FireDeath() //fire death because material change
    {
        Debug.Log("Reached here");
        body.GetComponent<SkinnedMeshRenderer>().material = burnMaterial;
        Death2();
        EnableRagdoll();

        if(EnemyType == 3)
        {
            GetComponent<AgentLinkMover>().enabled = false;
        }

        Debug.Log("Reached here 2");
    }

    private void OnTriggerStay(Collider other)
    {   if(!dead)
        {
            if (other.CompareTag("Barrier") && attackBarrier == true)
            {
                /*Debug.Log("attacking by skeleton");*/
                an.SetTrigger("Attack");
                other.GetComponent<BarrierScript>().DecreaseBarrierHealth(damageToBarrier);
                attackBarrier = false;
                Invoke("AttackBarrierTrue", damageToBarrierTime);
            }
        }
        
    }

    private void AttackBarrierTrue()
    {
        attackBarrier = true;
    }



/*    private void ResetHit()
    {
        hit = false;
    }*/

    //for skeleton. kinematic On.
/*    public void StopSkeleton() 
    {
        foreach (Rigidbody rb in ragdollBodies)
        {
            rb.useGravity = false;
            rb.isKinematic = true;
        }
    }*/
}