using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateTurrets : MonoBehaviour
{
    // Start is called before the first frame update
    Transform t;
    GameObject[] turrets;
    public float radius = 25;
    void Start()
    {
        t = this.transform;
        turrets = GameObject.FindGameObjectsWithTag("Turret");
    }

    // Update is called once per frame
    void Update()
    {
        //Disable turrets not in player range to improve performance
        foreach (GameObject turret in turrets)
        {
            if ((turret.transform.position - t.position).magnitude > radius)
            {
                if (!turret.GetComponent<Railgun>())
                {
                    turret.SetActive(false);
                }
            }
            else
            {
                turret.SetActive(true);
            }
        }
    }
}
