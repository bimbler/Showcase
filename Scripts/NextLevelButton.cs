using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevelButton : MonoBehaviour
{
    public void BtnPress()
    {
        GameObject.FindGameObjectWithTag("Manager").GetComponent<Manager>().LoadNextScene();
    }    
}
