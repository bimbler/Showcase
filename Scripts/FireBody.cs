using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBody : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject.Destroy(this.gameObject, 2f);
    }
}
