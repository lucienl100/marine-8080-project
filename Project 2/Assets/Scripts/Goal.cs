using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public SceneController sc;
    RadarRotate rr;
    public int levelNo;
    private float delay = 3f;
    void Start()
    {
        rr = this.GetComponent<RadarRotate>();
    }
    public void Update()
    {
        if (rr.inRange && Input.GetKeyDown(KeyCode.E))
        {
            rr.completed = true;
            rr.inst.SetActive(false);
            LoadNext();
        }
    }
    public void LoadNext() 
    {
        Debug.Log("Goal reached");
        StartCoroutine(TransitionDelay());
    }
    public IEnumerator TransitionDelay()
    {
        yield return new WaitForSeconds(1f);
        sc.WinScreen();
    }
}
