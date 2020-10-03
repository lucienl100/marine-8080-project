using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpDownMovement : MonoBehaviour
{
    Transform t;
    float hi;
    float lo;
    public float len = 5f;
    public float dir = 1f;
    public float speed = 10f;
    void Start() 
    {
        t = this.transform;
        hi = t.position.y + len;
        lo = t.position.y - len;
    }
    // Update is called once per frame
    void Update()
    {
        t.position += Vector3.up * Time.deltaTime * speed * dir;
        if (t.position.y > hi)
        {
            dir = -1f;
        }
        else if (t.position.y < lo)
        {
            dir = 1f;
        }
    }
}
