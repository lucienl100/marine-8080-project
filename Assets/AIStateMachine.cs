using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStateMachine : StateMachineBehaviour
{
    Transform player;
    public float spotRange = 15f;
    public float patrolRange = 10f;
    public float direction;
    private float duration = 3f;
    private float timer;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator.SetFloat("initialX", animator.transform.position.x);
        timer = duration;
        direction = Random.Range(0, 1) < 0.5 ? 1 : -1;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Mathf.Abs(player.position.x - animator.transform.position.x) < spotRange)
        {
            animator.SetBool("playerInRange", true);
        }
        if (direction == 1)
        {
            animator.transform.rotation = Quaternion.Euler(new Vector3(0f, 90f, 0f));
        }
        else
        {
            animator.transform.rotation = Quaternion.Euler(new Vector3(0f, 270f, 0f));
        }
        animator.transform.position += direction * new Vector3(5f, 0f, 0f) * Time.deltaTime;
        if (timer <= 0)
        {
            direction = -direction;
            timer = duration;
        }
        else
        {
            timer -= Time.deltaTime;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
