using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using MoreMountains.Feedbacks;
using TMPro;

public class BowScript : MonoBehaviour
{
    public GameObject arrowPrefab;
    public GameObject arrowHolder;

    public List<GameObject> arrows;
    public float turnSpeed = 1f;

    public LineRenderer bowString;

    private CinemachineVirtualCamera stationaryCamera;
    private CinemachineVirtualCamera followCamera;
    private int arrowCount = 0;
    private bool stationaryCam = true;
    private bool canFly = false;

    private Quaternion startRot;
    private bool aiming = true;
    private Vector3 bowStringCenter;
    private bool pullBack = false;

    public Vector3 bowStringStart;
    public Vector3 bowStringEnd;
    private float desDuration = 0.5f;
    private float elapsedTime = 0f;

    private bool aimed = false;

    private Vector3 arrowHolderEnd = new Vector3(-1.34f, 0, -0.1f);
    private Vector3 arrowHolderStart = new Vector3(1.3f, 0, -0.1f);
    public GameObject tutorialParent;
    private void Awake()
    {
        stationaryCamera = GameObject.FindGameObjectWithTag("StationaryCam").GetComponent<CinemachineVirtualCamera>();
        followCamera = GameObject.FindGameObjectWithTag("FollowCam").GetComponent<CinemachineVirtualCamera>();
    }

    // Start is called before the first frame update
    void Start()
    {
        arrowHolder.transform.localPosition = arrowHolderStart;
        startRot = transform.rotation;
        arrows.Add(Instantiate(arrowPrefab, arrowHolder.transform.position, arrowHolder.transform.rotation));
        SpawnArrow();
        if (GameObject.FindGameObjectWithTag("Manager").GetComponent<Manager>().tutorial == true)
        {
            tutorialParent.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && aiming)
        {
            elapsedTime = 0f;
            GameObject.FindGameObjectWithTag("ArrowPullbackFeedback").transform.GetComponent<MMFeedbacks>()?.PlayFeedbacks();
            aimed = true;
            pullBack = true;
        }
        
        if(aimed)
        {
            if (pullBack)
            {
                elapsedTime += Time.deltaTime;
                float percentageComplete = elapsedTime / desDuration;

                bowString.SetPosition(1, Vector3.Lerp(bowStringStart, bowStringEnd, percentageComplete));
                arrowHolder.transform.localPosition = Vector3.Lerp(arrowHolderStart, arrowHolderEnd, percentageComplete);
            }

            else
            {
                elapsedTime += Time.deltaTime;
                float percentageComplete = elapsedTime / (desDuration / 2);

                bowString.SetPosition(1, Vector3.Lerp(bowStringEnd, bowStringStart, percentageComplete));
            }
        }

        if (Input.GetMouseButtonUp(0) && canFly)
        {
            aiming = false;
            pullBack = false;
            elapsedTime = 0f;
            GameObject.FindGameObjectWithTag("ArrowLaunchedFeedback").transform.GetComponent<MMFeedbacks>()?.PlayFeedbacks();
            arrows[arrowCount - 1].GetComponent<ArrowMovementTest>().wakeArrow();
            ArrowAwake();
            canFly = false;
            if(GameObject.FindGameObjectWithTag("Manager").GetComponent<Manager>().tutorial==false)
            {
                tutorialParent.transform.GetChild(2).GetComponent<TMP_Text>().text = "Drag to Move & Activate Slow Motion";
                tutorialParent.transform.GetChild(3).GetComponent<Animator>().SetTrigger("Swipe");
                Invoke("RemoveTutorial", 2f);
            }
            
        }
    }
    void RemoveTutorial()
    {
        tutorialParent.SetActive(false);
        GameObject.FindGameObjectWithTag("Manager").GetComponent<Manager>().tutorial = true;
    }

    private void SpawnArrow()
    {
        aiming = true;
        arrows[arrowCount].transform.parent = arrowHolder.transform;
        followCamera.LookAt = arrows[arrowCount].transform;
        followCamera.Follow = arrows[arrowCount].transform;
        followCamera.ForceCameraPosition(Vector3.zero, Quaternion.identity);
        arrowCount++;
        canFly = true;
    }

    public void OnTargetHit()
    {
        arrowHolder.transform.localPosition = arrowHolderStart;
        transform.rotation = startRot;
        arrows.Add(Instantiate(arrowPrefab, arrowHolder.transform.position, arrowHolder.transform.rotation));
        Invoke("SpawnArrow", 1f);
        stationaryCam = true;
        SwitchPriority();
    }

    public void ArrowAwake()
    {
        stationaryCam = false;
        SwitchPriority();
    }

    private void SwitchPriority()
    {
        if (stationaryCam)
        {
            followCamera.Priority = 0;
            stationaryCamera.Priority = 1;
        }

        else
        {
            followCamera.Priority = 1;
            stationaryCamera.Priority = 0;
        }
    }
}
