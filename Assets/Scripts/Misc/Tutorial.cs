﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public GameObject[] instructions;
    public GameObject cutCamera;
    public GameObject mainCamera;
    public GameObject hud;
    private int index;
    public Shooting s;
    public float delay = 1f;
    public float intDelay = 10f;
    private float timer;
    bool done = false;
    SceneController sc;
    // Start is called before the first frame update
    void Start()
    {
        sc = this.gameObject.GetComponent<SceneController>();
        timer = intDelay;
        index = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!sc.paused)
        {
            for (int i = 0; i < instructions.Length; i++)
            {
                if (i == index)
                {
                    instructions[i].SetActive(true);
                }
                else
                {
                    instructions[i].SetActive(false);
                }
            }
            if (index == 5 && Input.GetKey(KeyCode.Mouse0))
            {
                done = true;
                timer = 2f;
            }
            if (index == 0)
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    hud.SetActive(true);
                    cutCamera.SetActive(false);
                    mainCamera.SetActive(true);
                    index++;
                    timer = delay;
                }
                timer -= Time.deltaTime;
            }
            if (index == 1 && (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A)))
            {
                if (timer <= 0f)
                {
                    index++;
                    timer = delay;
                }
                timer -= Time.deltaTime;
            }
            if (index == 2 && Input.GetKeyDown(KeyCode.Space))
            {
                done = true;
            }
            if (index == 3 && Input.GetKeyDown(KeyCode.LeftShift))
            {
                done = true;
            }
            if (index == 4 && Input.GetKeyDown(KeyCode.Mouse0))
            {
                index++;
            }
            if (index == 6 && Input.GetKeyDown(KeyCode.Mouse0))
            {
                index++;
            }
            if (done)
            {
                if (timer <= 0f)
                {
                    index++;
                    timer = 2f;
                    done = false;
                }
                timer -= Time.deltaTime;
            }

            if (index == 7)
            {
                if (timer <= 0f)
                {
                    instructions[7].SetActive(false);
                    this.enabled = false;
                    PlayerPrefs.SetInt("tutorial1", 1);
                }
                timer -= Time.deltaTime;
            }
        }
    }
}
