using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuUIScript : MonoBehaviour
{
    public Manager manager;
    public GameObject OfflineEarning;
    public GameObject Damage;

    private int baseCost = 5;
    private int offlineEarningCost = 0;
    private int damageCost = 0;

    // Start is called before the first frame update
    void Start()
    {
        offlineEarningCost += (int)Mathf.Pow(baseCost, manager.offlineEarningLevel);
        Debug.Log("Offline Earning Cost: " + offlineEarningCost);
    }


    public void UpgradePress()
    {
        manager.UpgradeOfflineEarning();
        offlineEarningCost += (int)Mathf.Pow(baseCost, manager.offlineEarningLevel);
        Debug.Log("Offline Earning Cost: " + offlineEarningCost);
    }
}
