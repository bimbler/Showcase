using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpScript : MonoBehaviour
{
    public int arrowType;

    public void PickUp(ArrowMovementTest amt)
    {
        amt.arrowType = arrowType;
        transform.parent.GetComponent<PowerUpParent>().InvokeSpawnPowerUp();
        Destroy(this.gameObject);
    }

    public void PickUp(NewArrowScript nas)
    {
        nas.arrowType = arrowType;
        transform.parent.GetComponent<PowerUpParent>().InvokeSpawnPowerUp();
        Destroy(this.gameObject);
    }
}
