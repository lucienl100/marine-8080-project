using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activate : MonoBehaviour
{
    public LookAtPlayer lap;
    public Transform player;
    Transform t;
    // Start is called before the first frame update
    void Start()
    {
        t = this.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if ((player.position - t.position).magnitude < 30f)
        {
            lap.enabled = true;
        }
        else
        {
            lap.enabled = false;
        }
    }
}
