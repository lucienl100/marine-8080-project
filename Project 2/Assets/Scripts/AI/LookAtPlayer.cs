using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    // Update is called once per frame
    Quaternion faceRotation = Quaternion.Euler(new Vector3(0f, 90f, 0f));
    public Transform player;
    public Transform groundCheck;
    public Animator anim;
    Rigidbody rb;
    public bool inRange;
    private float spotRange = 18f;
    public float minRange = 3f;
    public float patrolStep = 3f;
    private float turningTime = 0.3f;
    private float turningTimer = 0.3f;
    private bool playerIsRight;
    private float patrolTimer;
    bool facingRight;
    Vector3 dirToLook;
    Vector3 slideDirToLook;
    Transform t;
    public LayerMask layerMask;
    public LayerMask groundLayer;
    public Transform weapon;
    public Transform chest;
    Sliding playerslide;
    public Vector3 startPos;
    float delay = 0.5f;
    float timer;
    void Awake()
    {
        rb = this.GetComponent<Rigidbody>();
        timer = delay;
        t = this.transform;
        startPos = t.position;
        playerslide = player.GetComponent<Sliding>();
        Physics.IgnoreCollision(player.GetComponent<Collider>(), GetComponent<Collider>());
        patrolTimer = patrolStep;
        playerIsRight = (player.position.x - t.position.x) > 0 ? true : false;
        facingRight = t.rotation.eulerAngles.y == 90f ? true : false;
    }
    void Update()
    {
        dirToLook = player.position - t.position;
        slideDirToLook = new Vector3(player.position.x, player.position.y - 1f, player.position.z) - t.position;
        RaycastHit hit;
        if (!playerslide.isSliding && (Physics.Raycast(t.position, dirToLook, out hit, Mathf.Infinity, layerMask)) || (playerslide.isSliding && Physics.Raycast(t.position, slideDirToLook, out hit, Mathf.Infinity, layerMask)))
        {
            if ((player.position - t.position).magnitude < spotRange && hit.transform.tag == "Player")
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
                timer = -1f;
            }
            else
            {
                inRange = false;
                Patrol();
                anim.SetBool("Stop", false);
                anim.SetBool("playerInRange", false);
            }
        }
        else
        {
            inRange = false;
            Patrol();
            anim.SetBool("Stop", false);
            anim.SetBool("playerInRange", false);
        }
        if (timer > 0f)
        {
            timer -= Time.deltaTime;
        }
    }
    void LateUpdate()
    {
        //Since we are not using the physics velocity system but rigidbodies, set the velocity to zero as collision between other enemies will create force.
        rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
        if (inRange)
        {
            Quaternion rotation;
            if (playerslide.isSliding)
            {
                rotation = Quaternion.LookRotation(slideDirToLook);
            }
            else
            {
                rotation = Quaternion.LookRotation(dirToLook);
            }
            
            chest.rotation = Quaternion.Euler(rotation.eulerAngles.x + 15f, chest.eulerAngles.y + 35f, rotation.eulerAngles.z);
        }
    }
    void FollowPlayer()
    {
        //If the player is in range, follow the player, since the enemy will always face towards the player, we just need to translate forward
        if (CheckGround() && Mathf.Abs(player.position.x - t.position.x) > minRange)
        {
            anim.SetBool("Stop", false);
            t.position += new Vector3(t.forward.x, 0f, 0f) * Time.deltaTime * 3f;
            //Account for any errors in the z axis
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
        //If the player moves to the left of the player, turn left.
        if (t.position.x < player.position.x - 0.1f)
        {
            if (!playerIsRight)
            {
                playerIsRight = true;
                turningTimer = turningTime;
            }
            faceRotation = Quaternion.Euler(new Vector3(0f, 90f, 0f));

        }
        //If the player moves the the right of the player, turn right.
        else if (t.position.x > player.position.x + 0.1f)
        {
            faceRotation = Quaternion.Euler(new Vector3(0f, 270f, 0f));
            if (playerIsRight)
            {
                playerIsRight = false;
                turningTimer = turningTime;
            }
        }
        //Turn the transform
        t.eulerAngles = new Vector3(t.eulerAngles.x, Mathf.Lerp(t.eulerAngles.y, faceRotation.eulerAngles.y, Time.deltaTime * 15.0f), t.eulerAngles.z);
        //Account for slerp errors
        if (t.eulerAngles.y > 270 - 10f && t.eulerAngles.y < 270 + 10f && !playerIsRight)
        {
            t.eulerAngles = new Vector3(0f, 270f, 0f);
        }
        else if (t.eulerAngles.y > 90 - 10f && t.eulerAngles.y < 90 + 10f && playerIsRight)
        {
            t.eulerAngles = new Vector3(0f, 90f, 0f);
        }
    }
    void Patrol()
    {
        //Code for going back and forth around the enemies location
        if (timer <= 0f && !CheckGround())
        {
            timer = delay;
            patrolTimer = -1f;
        }
        if (patrolTimer <= 0)
        {
            faceRotation = Quaternion.Euler(new Vector3(0f, faceRotation.eulerAngles.y + 180f, 0f));
            patrolTimer = patrolStep;
            turningTimer = turningTime;
            facingRight = !facingRight;
        }
        else
        {
            patrolTimer -= Time.deltaTime;
        }
        if (turningTimer <= 0)
        {
            t.position += t.forward * Time.deltaTime * 3f;
            t.position = new Vector3(t.position.x, t.position.y, -2.5f);
        }
        else
        {
            t.rotation = Quaternion.Slerp(t.rotation, faceRotation, Time.deltaTime * 15.0f);

            turningTimer -= Time.deltaTime;
        }
        //Account for slerp errors
        if (t.eulerAngles.y > 270 - 10f && t.eulerAngles.y < 270 + 10f && !facingRight)
        {
            t.eulerAngles = new Vector3(0f, 270f, 0f);
        }
        else if (t.eulerAngles.y > 90 - 10f && t.eulerAngles.y < 90 + 10f && facingRight)
        {
            t.eulerAngles = new Vector3(0f, 90f, 0f);
        }
    }
    bool CheckGround()
    {
        //Check if there is ground in front of the enemy
        if (Physics.CheckSphere(groundCheck.position, 0.1f, groundLayer))
        {
            return true;
        }
        else
        {
            Debug.Log("no ground");
            return false;
        }
    }
}
