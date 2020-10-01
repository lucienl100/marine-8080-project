using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    public Animator anim;
    public LookAtMouse lm;
    public GameObject player;
    private Movement mv;
    private float delay = 0.05f;
    private float timer1;
    private float timer2;
    // Start is called before the first frame update
    void Start()
    {
        timer1 = delay;
        timer2 = delay;
        mv = player.GetComponent<Movement>();
    }

    // Update is called once per frame
    void Update()
    {
        float dir = (lm.playerIsRight) ? -1 : 1;
        anim.SetFloat("Speed", dir * mv.velocity.x);
        anim.SetBool("inAir", CheckDelayInAir() && !CheckDelayLand());
    }
    bool CheckDelayInAir()
    {
        timer1 -= Time.deltaTime;
        if (timer1 <= 0 && mv.inAir)
        {
            return true;
        }
        else if (!mv.inAir)
        {
            timer1 = delay;
        }
        return false;
    }
    bool CheckDelayLand()
    {
        timer2 -= Time.deltaTime;
        if (timer2 <= 0 && !mv.inAir)
        {
            return true;
        }
        else if (!mv.inAir)
        {
            timer2 = delay;
        }
        return false;
    }
}
