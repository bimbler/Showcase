using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using UnityEngine.UI;
public class ArrowMovementTest : MonoBehaviour
{
    public float movSpeed=30;
    public float turnSpeed;
    public int arrowType=0;
    public GameEvent OnTargetHit;
    public GameObject fire;
    public GameObject explosion;
    public GameObject Electricity;
    public GameObject fireBoss;
    public GameObject electricityBoss;
    public float sloMoTime = 3;
    public float movSpeedMultiplier = 3;
    public float turnSpeedMultipler = 4;
    private bool isFlying = false;
    private bool targetHit = false;
    private GameObject anchor = null;
    private Vector3 touchStart;
    private Vector3 newPos;
    private Rigidbody rb;
    private string hitObject;
    private bool slowMotion = true;
    public Slider slowMoBar;
    public Text slowMoText;
    private bool bloodSpawned = false;
    private bool sticked = false;
    int layerMask;

    // Start is called before the first frame update
    void Start()
    {
        sticked = false;
        slowMoBar = GameObject.Find("SlowMotionSlider").GetComponent<Slider>();
        slowMoText = GameObject.Find("SlowMotionSlider").transform.GetChild(3).GetComponent<Text>();
        slowMoBar.value =sloMoTime;
        slowMoText.text = slowMoBar.value.ToString();
        isFlying = false;
        targetHit = false;
        rb = GetComponent<Rigidbody>();
        Vector3 targetVelocity = transform.up * movSpeed;
        layerMask = 1 << 6;
        layerMask = ~layerMask;
    }       
    
    private void FixedUpdate() //Arrow Interactions
    {   
        RaycastHit hit;

        //Collision happens (enemy, ground etc)
        if (Physics.Raycast(transform.position, transform.up, out hit, 0.6f,layerMask) && targetHit == false)
        {
            hitObject = hit.transform.tag;

            switch(hitObject)
            {
                case "Enemy":
                    #region ArrowType Description
                    //Arrow 0 = Default/Piercing
                    //Arrow 1 = Molly
                    //Arrow 2 = Electricity
                    //Arrow 3 = Explosion
                    #endregion
                    switch (arrowType)
                    {
                        case 1:
                            Debug.Log("Fire hit character ");
                            Instantiate(fire, hit.transform.root.position, Quaternion.identity);
                            StickArrow(hit);
                            break;

                        case 2:
                            if (hit.transform.root.GetComponent<EnemyManager>().dead == false)
                            {

                                GameObject.FindGameObjectWithTag("LightningFeedback").transform.GetComponent<MMFeedbacks>()?.PlayFeedbacks();
                                hit.transform.root.GetComponent<EnemyManager>().ElectricityStart();
                                StickArrow(hit);
                            }
                            break;

                        case 3:
                            if (hit.transform.root.GetComponent<EnemyManager>().dead == false)
                            {
                                Debug.Log("calling him");
                                Explode();
                                StickArrow(hit);
                            }
                            break;

                        default:
                            GameObject.FindGameObjectWithTag("ArrowPierceFeedback").transform.GetComponent<MMFeedbacks>()?.PlayFeedbacks();
                            hit.transform.root.GetComponent<EnemyManager>().Death(hit.transform, transform.up);
                            hit.transform.root.GetComponent<EnemyManager>().SpawnBlood(hit.point);
                            break;
                    }
                    break;

                case "Boss":
                    #region ArrowType Description
                    //Arrow 0 = Default/Piercing
                    //Arrow 1 = Molly
                    //Arrow 2 = Electricity
                    //Arrow 3 = Explosion
                    #endregion
                    switch (arrowType)
                    {
                        case 1:
                            Debug.Log("Fire hit Boss ");
                            Instantiate(fireBoss, hit.transform.root.position + new Vector3(0, 1, 0), Quaternion.identity);
                            hit.transform.root.GetComponent<BossManager>().HitByArrow(3, 1);
                            StickArrow(hit);
                            break;

                        case 2:
                            GameObject.FindGameObjectWithTag("LightningFeedback").transform.GetComponent<MMFeedbacks>()?.PlayFeedbacks();
                            Instantiate(electricityBoss, hit.transform.root.position + new Vector3(0, 2.6f, 0), Quaternion.identity);
                            hit.transform.root.GetComponent<BossManager>().HitByArrow(3, 2);
                            StickArrow(hit);

                            break;

                        case 3:

                            Instantiate(explosion, transform.position, Quaternion.identity);
                            hit.transform.root.GetComponent<BossManager>().HitByArrow(3, 3);
                            StickArrow(hit);

                            break;

                        default:
                            hit.transform.root.GetComponent<BossManager>().HitByArrow(1, 0);
                            hit.transform.root.GetComponent<BossManager>().SpawnBlood(hit.point);
                            /*SpawnBlood();*/
                            break;
                    }
                    break;

                case "PickUp":
                    hit.transform.GetComponent<PickUpScript>().PickUp(this);
                    break;

                case "SkeletonNew":
                    hit.transform.GetComponent<Rigidbody>().AddForce(transform.position * 3, ForceMode.Impulse);
                    break;

                case "LevelEdge":
                    StickArrow(hit);
                    break;

                default:
                    transform.position = hit.point;/*hit.transform.position*/
                    switch (arrowType)
                    {
                        case 1:
                            Debug.Log("Fire hit GROUND ");
                            GameObject.FindGameObjectWithTag("MollyFeedback").transform.GetComponent<MMFeedbacks>()?.PlayFeedbacks();
                            Instantiate(fire, transform.position, Quaternion.identity);
                            break;

                        case 3:
                            Debug.Log("calling him");
                            Explode();
                            break;
                    }
                    //StickArrow
                    StickArrow(hit);
                    break;
            }
        }

        //While flying
        if (isFlying)
        {
            rb.velocity = new Vector3(0, 0, 0);
            rb.angularVelocity = new Vector3(0, 0, 0);
            
            var locVel = transform.InverseTransformDirection(rb.velocity);
            locVel.y = movSpeed;
            rb.velocity = transform.TransformDirection(locVel);
        }

        //Arrow Sticks (Flying Ended)
        else if(!isFlying && targetHit)
        {
            transform.position = anchor.transform.position;
            transform.rotation = anchor.transform.rotation;
            targetHit = true;
        }
    }

    // Update is called once per frame
    void Update() //Arrow Movement
    {
        if(isFlying)
        {
            //If slowmo available, activate slomo. Get touchstart
            if (Input.GetMouseButtonDown(0) && sloMoTime > 0)
            {
                StartSlowMotion();
                touchStart = Camera.main.ScreenToViewportPoint(Input.mousePosition) * 3000;
            }

            //if no slomo, just get touchstart
            else if (Input.GetMouseButtonDown(0) && sloMoTime < 0)
            {
                touchStart = Camera.main.ScreenToViewportPoint(Input.mousePosition)* 3000;
            }

            //Inflight
            if (Input.GetMouseButton(0)) //new mechanic second part
            {
                newPos = Camera.main.ScreenToViewportPoint(Input.mousePosition) * 3000; //original 2000  new 1500
                Vector3 direction = newPos - touchStart;
                transform.RotateAround(transform.position, transform.right, direction.y * turnSpeed * 90 * Time.deltaTime);
                transform.Rotate(0, direction.x * turnSpeed * 90 * Time.deltaTime, 0, Space.World);
                //transform.Rotate(direction.y * turnSpeed * 90 * Time.deltaTime, 0, direction.x * turnSpeed * 90 * Time.deltaTime); this has all roll yaw pitch
                touchStart = newPos;
                sloMoTime -= Time.unscaledDeltaTime;
                slowMoBar.value -= Time.unscaledDeltaTime;
                slowMoText.text = ((int)slowMoBar.value).ToString();
                if (sloMoTime < 0 && slowMotion)
                {
                    StopSlowMotion();
                    turnSpeed /= turnSpeedMultipler;
                    slowMotion = false;
                }
            }

            if (Input.GetMouseButtonUp(0) && slowMotion)
            {
                StopSlowMotion();
            }
        }
    }


    private void StartSlowMotion()
    {
        GameObject.FindGameObjectWithTag("NormalSpeedFeedback").transform.GetComponent<MMFeedbacks>()?.StopFeedbacks();
        Time.timeScale = 0.25f;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
        movSpeed /= movSpeedMultiplier;
        GameObject.FindGameObjectWithTag("SloMoFeedback").transform.GetComponent<MMFeedbacks>()?.PlayFeedbacks();
    }

    private void StopSlowMotion()
    {
        GameObject.FindGameObjectWithTag("SloMoFeedback").transform.GetComponent<MMFeedbacks>()?.StopFeedbacks();
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
        movSpeed *= movSpeedMultiplier;
        GameObject.FindGameObjectWithTag("NormalSpeedFeedback").transform.GetComponent<MMFeedbacks>()?.PlayFeedbacks();
    }

    private void StickArrow(RaycastHit hit)
    {
        if(!sticked)
        {
            GameObject.FindGameObjectWithTag("SloMoFeedback").transform.GetComponent<MMFeedbacks>()?.StopFeedbacks();
            GameObject.FindGameObjectWithTag("ArrowLandedFeedback").transform.GetComponent<MMFeedbacks>()?.PlayFeedbacks();
            GameObject anchor = new GameObject("ARROW_ANCHOR");
            anchor.transform.position = transform.position;
            anchor.transform.rotation = transform.rotation;
            anchor.transform.parent = hit.transform;
            this.anchor = anchor;

            isFlying = false;
            Destroy(rb);
            Debug.Log("Hit");
            Time.timeScale = 1f;
            Time.fixedDeltaTime = 0.02f;
            turnSpeed /= 4;
            targetHit = true;
            OnTargetHit.Raise();
            transform.parent = null;
            sticked = true;
        }
    }

    private void Explode()
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
        Debug.Log("Calling explosion");
        GameObject.FindGameObjectWithTag("ExplosionFeedback").transform.GetComponent<MMFeedbacks>()?.PlayFeedbacks();
        Collider[] bombObjects = Physics.OverlapSphere(transform.position, 5);
        foreach (Collider c in bombObjects)
        {
            if (c.gameObject.transform.root.CompareTag("EnemyParent") && !c.gameObject.transform.root.GetComponent<EnemyManager>().hitByBomb)
            {
                Debug.Log("happening on this object " + c.transform.name);
                Rigidbody rb = c.GetComponent<Rigidbody>();
                if (rb != null/* && c.gameObject.transform.root.GetComponent<EnemyManager>().hitByBomb == false*/)
                {
                    c.gameObject.transform.root.GetComponent<EnemyManager>().Death2();
                    c.gameObject.transform.root.GetComponent<EnemyManager>().EnableRagdoll();
                    c.gameObject.transform.root.GetComponent<EnemyManager>().hitByBomb = true;

                    if (c.gameObject.transform.root.GetComponent<EnemyManager>().EnemyType == 1)
                    {
                        c.gameObject.transform.root.GetComponent<EnemyManager>().ExplodeSkeleton(transform.position);
                    }

                    else
                    {
                        rb.AddExplosionForce(1000, transform.position, 5, 3, ForceMode.Impulse);
                    }
                }
            }
        }
    }

    public void wakeArrow()
    {
        Invoke("wakeInvoke", 0.1f);
    }

    public void wakeInvoke()
    {
        isFlying = true;
    }
}
