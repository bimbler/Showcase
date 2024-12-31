using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject.Destroy(this.gameObject, 5f); 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && other.transform.root.GetComponent<EnemyManager>().dead == false)
        {
            other.transform.root.GetComponent<EnemyManager>().FireStart();
        }
    }
}
