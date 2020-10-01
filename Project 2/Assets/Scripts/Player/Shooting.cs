using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public Vector3 target;
    public LookAtMouse lm;
    Transform gun;
    // Start is called before the first frame update
    void Start()
    {
        gun = this.transform;
    }

    // Update is called once per frame
    void Update()
    {
        //Here you go Nate, cursor position with correct depth.
        target = lm.mouseWorld;
    }
}
