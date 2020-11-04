using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level05Tutorial : MonoBehaviour
{
    public GameObject[] instructions;
    public Shooting s;
    private int index;
    private float timer = 2f;
    bool done = false;
    void Start()
    {
        s.enabled = false;
        index = 0;
    }
    // Update is called once per frame
    void Update()
    {
        if (index == 1 && Input.GetKeyDown(KeyCode.Mouse0))
        {
            instructions[1].SetActive(false);
            Invoke("EnableShooting", 0.1f);
        }
        if (index == 0 && Input.GetKeyDown(KeyCode.Mouse0))
        {
            instructions[0].SetActive(false);
            instructions[1].SetActive(true);
            index++;
        }
    }
    void EnableShooting()
    {
        s.enabled = true;
    }
}

