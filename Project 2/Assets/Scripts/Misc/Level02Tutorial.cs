using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level02Tutorial : MonoBehaviour
{
    public GameObject[] instructions;
    private int index;
    private float timer = 2f;
    bool done = false;
    void Start()
    {
        index = 0;
    }
    // Update is called once per frame
    void Update()
    {
        if (index == 2 && Input.GetKeyDown(KeyCode.Mouse0))
        {
            instructions[2].SetActive(false);
            done = true;
        }
        if (index == 1 && Input.GetKeyDown(KeyCode.Mouse0))
        {
            instructions[1].SetActive(false);
            instructions[2].SetActive(true);
            index++;
        }
        if (index == 0 && Input.GetKeyDown(KeyCode.Mouse0))
        {
            instructions[0].SetActive(false);
            instructions[1].SetActive(true);
            index++;
        }
    }
}
