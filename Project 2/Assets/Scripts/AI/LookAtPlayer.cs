using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    // Update is called once per frame
    Quaternion faceRotation = Quaternion.Euler(new Vector3(0f, 90f, 0f));
    public Transform player;
    public Animator anim;
    public bool inRange;
    public float spotRange = 15f;
    public float minRange = 3f;
    public float patrolStep = 3f;
    private float turningTime = 0.3f;
    private float turningTimer = 0.3f;
    private bool playerIsRight;
    private float patrolTimer;
    Transform t;
    public Transform weapon;
    public Transform chest;
    void Start()
    { 
        patrolTimer = patrolStep;
        t = this.transform;
        playerIsRight = (player.position.x - t.position.x) > 0 ? true : false;
    }
    void Update()
    {
        if (Mathf.Abs(player.position.x - t.position.x) < spotRange)
        {
            anim.SetBool("playerInRange", true);
            inRange = true;
            CheckRotation();
            if (turningTimer <= 0)
            {
                FollowPlayer();
            }
            else
            {
                turningTimer -= Time.deltaTime;
            }
        }
        else
        {
            inRange = false;
            Patrol();
            anim.SetBool("playerInRange", false);
        }
    }
    void LateUpdate()
    {
        Vector3 dirToLook = player.position - t.position;
        Quaternion rotation = Quaternion.LookRotation(dirToLook);
        chest.rotation = Quaternion.Euler(rotation.eulerAngles.x + 15f, chest.eulerAngles.y + 35f, rotation.eulerAngles.z);
    }
    void FollowPlayer()
    {
        if (Mathf.Abs(player.position.x - t.position.x) > minRange)
        {
            anim.SetBool("Stop", false);
            t.position += new Vector3(t.forward.x, 0f, 0f) * Time.deltaTime * 3f;
            t.position = new Vector3(t.position.x, t.position.y, -2.5f);
        }
        else
        {
            anim.SetBool("Stop", true);
        }
    }
    void CheckRotation()
    {
        patrolTimer = patrolStep;
        if (t.position.x < player.position.x - 0.1f)
        {
            if (!playerIsRight)
            {
                playerIsRight = true;
                turningTimer = turningTime;
            }
            faceRotation = Quaternion.Euler(new Vector3(0f, 90f, 0f));

        }
        else if (t.position.x > player.position.x + 0.1f)
        {
            faceRotation = Quaternion.Euler(new Vector3(0f, 270f, 0f));
            if (playerIsRight)
            {
                playerIsRight = false;
                turningTimer = turningTime;
            }
        }
        t.rotation = Quaternion.Slerp(t.rotation, faceRotation, Time.deltaTime * 15.0f);
        t.eulerAngles = new Vector3(0f, t.eulerAngles.y, 0f);
    }
    void Patrol()
    {
        if (patrolTimer <= 0)
        {
            faceRotation = Quaternion.Euler(new Vector3(0f, faceRotation.eulerAngles.y + 180f, 0f));
            patrolTimer = patrolStep;
            turningTimer = turningTime;
        }
        else
        {
            patrolTimer -= Time.deltaTime;
        }
        if (turningTimer <= 0)
        {
            t.position += t.forward * Time.deltaTime * 3f;
        }
        else
        {
            t.rotation = Quaternion.Slerp(t.rotation, faceRotation, Time.deltaTime * 15.0f);
            turningTimer -= Time.deltaTime;
        }
    }
}
