using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    float timer = 30f;
    // Update is called once per frame
    void Update()
    {
        if (timer <= 0f || Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }
        timer -= Time.deltaTime;
    }
}
