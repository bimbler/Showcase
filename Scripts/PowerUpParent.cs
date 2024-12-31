using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpParent : MonoBehaviour
{
    public GameObject powerUp;
    public float spawnDelay = 1f;
    // Start is called before the first frame update
    void Start()
    {
        InvokeSpawnPowerUp();
    }

    public void InvokeSpawnPowerUp()
    {
        Invoke("SpawnPowerUp", spawnDelay);
    }

    private void SpawnPowerUp()
    {
        GameObject x = Instantiate(powerUp, transform.position, Quaternion.identity);
        x.transform.parent = transform;
    }
}
