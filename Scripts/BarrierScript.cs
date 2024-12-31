using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierScript : MonoBehaviour
{
    public float barrierHealth = 100;
    private Manager manager;
    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("Manager").GetComponent<Manager>();
    }

    public void DecreaseBarrierHealth(float health)
    {
        manager.decreaseBarrierHealth();
        /*barrierHealth -= health;*/
        /*Debug.Log("barrier health is" + barrierHealth);*/
        if (manager.barrierHealth <= 0)
        {
            GameObject.Destroy(this.gameObject);
        }
    }

    public void DecreaseByProjectile(float health)
    {
        manager.decreaseBarrierHealth();
        /*barrierHealth -= health;*/
        /*Debug.Log("barrier health is" + barrierHealth);*/
        if (manager.barrierHealth <= 0)
        {
            GameObject.Destroy(this.gameObject);
        }
    }
}
