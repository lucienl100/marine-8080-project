using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public SceneController sc;
    RadarRotate rr;
    public AudioSource sfx;
    public int levelNo;
    private float delay = 3f;
    void Start()
    {
        rr = this.GetComponent<RadarRotate>();
    }
    public void Update()
    {
        //If the player is in range of the goal and E is pressed, finish the level
        if (rr.inRange && Input.GetKeyDown(KeyCode.E))
        {
            sfx.Play();
            rr.completed = true;
            rr.inst.SetActive(false);
            LoadNext();
        }
    }
    public void LoadNext() 
    {
        Debug.Log("Goal reached");
        sc.WinScreen();
    }
}
