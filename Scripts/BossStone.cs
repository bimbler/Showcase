using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStone : MonoBehaviour
{
    public GameObject TargetObjectTF;
    float LaunchAngle = 45f;
    Rigidbody rigid;
    public GameObject rockParticles;
    Manager manager;
    // Start is called before the first frame update
    void Start()
    {
        TargetObjectTF = GameObject.FindGameObjectWithTag("Barrier");
        manager = GameObject.FindGameObjectWithTag("Manager").GetComponent<Manager>();
        rigid = GetComponent<Rigidbody>();
        transform.LookAt(TargetObjectTF.transform);
        Launch();

    }
    private void OnCollisionEnter(Collision collision)
    {
        manager.decreaseBarrierHealth();
        Instantiate(rockParticles, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    void Launch()
    {
        // think of it as top-down view of vectors: 
        //   we don't care about the y-component(height) of the initial and target position.
        Vector3 projectileXZPos = new Vector3(transform.position.x, 0.0f, transform.position.z);
        Vector3 targetXZPos = new Vector3(TargetObjectTF.transform.position.x, 0.0f, TargetObjectTF.transform.position.z);

        // rotate the object to face the target
        transform.LookAt(targetXZPos);

        // shorthands for the formula
        float R = Vector3.Distance(projectileXZPos, targetXZPos);
        float G = Physics.gravity.y;
        float tanAlpha = Mathf.Tan(LaunchAngle * Mathf.Deg2Rad);
        float H = TargetObjectTF.transform.position.y - transform.position.y;

        // calculate the local space components of the velocity 
        // required to land the projectile on the target object 
        float Vz = Mathf.Sqrt(G * R * R / (2.0f * (H - R * tanAlpha)));
        float Vy = tanAlpha * Vz;

        // create the velocity vector in local space and get it in global space
        Vector3 localVelocity = new Vector3(0f, Vy, Vz);
        Vector3 globalVelocity = transform.TransformDirection(localVelocity);

        // launch the object by setting its initial velocity and flipping its state
        rigid.velocity = globalVelocity;
        /*bTargetReady = false;*/
    }
}
