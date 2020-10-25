using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Randomthing : MonoBehaviour
{
    public GameObject smg;
    public GameObject rifle;
    public GameObject railgun;
    public GameObject shotgun;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            rifle.SetActive(false);
            smg.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            smg.SetActive(false);
            shotgun.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            shotgun.SetActive(false);
            railgun.SetActive(true);
        }
    }
}
