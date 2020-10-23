using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateTurrets : MonoBehaviour
{
    // Start is called before the first frame update
    Transform t;
    GameObject[] turrets;
    void Start()
    {
        t = this.transform;
        turrets = GameObject.FindGameObjectsWithTag("Turret");
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject turret in turrets)
        {
            if ((turret.transform.position - t.position).magnitude > 25f)
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
