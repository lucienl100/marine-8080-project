using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossEnding : MonoBehaviour
{
    public GameObject instructions;
    public Animator anim;
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
        if (timer <= 0f && done == false)
        {
            instructions.SetActive(true);
            done = true;
            Invoke("FadeOut", 5f);
        }
        timer -= Time.deltaTime;
    }
    void FadeOut()
    {
        Invoke("Credits", 2f);
        anim.SetTrigger("FadeOut");
    }
    void Credits()
    {
        SceneManager.LoadScene(7);
    }
}
