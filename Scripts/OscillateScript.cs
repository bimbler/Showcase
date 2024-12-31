using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OscillateScript : MonoBehaviour
{
    public float amplitude = 1f;
    public float frequency = 1f;
    public float offset;
    void Update()
    {
        transform.position = new Vector3(transform.position.x, (Mathf.Sin(Time.time * frequency + offset) * amplitude) + transform.parent.position.y, transform.position.z);//changed to move clouds in scene2 (commented parent)
    }
}
