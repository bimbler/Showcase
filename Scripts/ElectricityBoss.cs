using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricityBoss : MonoBehaviour
{
    GameObject boss;
    // Start is called before the first frame update
    void Start()
    {
        boss = GameObject.FindGameObjectWithTag("BossParent");
        Destroy(this.gameObject, 2f); 
    }
    private void FixedUpdate()
    {
        transform.position = boss.transform.position + new Vector3(0, 2.6f, 0);
    }
}
