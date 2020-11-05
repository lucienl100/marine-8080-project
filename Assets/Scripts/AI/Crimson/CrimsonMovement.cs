using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrimsonMovement : MonoBehaviour
{
    Transform t;
    // Start is called before the first frame update
    void Start()
    {
        t = this.transform;
    }

    // Update is called once per frame
    void Update()
    {
        t.position += t.forward * Time.deltaTime;
    }
}
