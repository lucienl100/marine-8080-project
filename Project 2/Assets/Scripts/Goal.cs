using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public SceneController sc;
    public int levelNo;
    public void OnTriggerEnter(Collider c) 
    {
        Debug.Log("Goal reached");
        if (c.gameObject.tag == "Player")
        {
            
            sc.LoadNextScene(levelNo);
        }
    }
}
