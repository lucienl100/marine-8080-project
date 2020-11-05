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
        //Activate LookAtPlayer script when the player is close enough
        if ((player.position - t.position).magnitude < 35f)
        {
            lap.enabled = true;
        }
        else
        {
            lap.enabled = false;
        }
    }
}
