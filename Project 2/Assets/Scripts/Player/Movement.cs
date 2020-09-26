using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public Vector3 velocity;
    public Vector3 additionalV;
    public Vector3 acceleration;
    [Range(0, 50)] public float speed;
    public float jumpSpeed;
    public float gravity = -9.81f;
    public bool inAir;
    public bool inJump;
    public CharacterController cc;
    public Animator anim;
    public Transform groundCheck;
    public Transform model;
    public Vector3 groundCheckPosition;
    public LayerMask groundMask;
    float playerHeight = 1.84f;

    float jumptime = 0.8f;
    public bool initialJump;
    bool landing = false;
    float timer;

    // Start is called before the first frame update
    void Start()
    {
        velocity = Vector3.zero;
        timer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        CalculateAirborne();
        Move();
        CalculateGravity();
        cc.Move(velocity * Time.deltaTime);
        HandleAdditionalV();
        if (landing)
        {
            NeutralStateTimer();
        }
        else if (inAir)
        {
            JumpLandingTimer();
        }
        ChangeRotation();
        anim.SetFloat("Speed", Mathf.Abs(velocity.x));
        anim.SetBool("inAir", inAir);
        groundCheckPosition = groundCheck.position;
    }

    void Move()
    {
        float x = Input.GetAxis("Horizontal");
        velocity.x = x * speed + additionalV.x;
        if (Input.GetKey(KeyCode.Space))
        {
            if (!inAir && Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
                initialJump = true;
            }
        }
        //  Change the jump height based on how long the player is holding the jump button on the first jump
        else if (initialJump && inJump && velocity.y > 0 && velocity.y < jumpSpeed * 0.75f)
        {
            velocity.y -= jumpSpeed * 0.05f;
        }
        velocity.z = 0;

    }

    public void Jump()
    {
        //  Prevents height from decaying if it is not the first jump
        initialJump = false;
        timer = jumptime;
        velocity.y = jumpSpeed;
        inJump = true;
    }

    void CalculateAirborne()
    {

        if (Physics.CheckSphere(groundCheck.position, 0.25f, groundMask))
        {
            inAir = false;
            if (velocity.y < 0)
            {
                inJump = false;
                initialJump = false;
            }
        }
        else
        {
            inAir = true;
        }
    }

    void CalculateGravity()
    {
        velocity.y += gravity * Time.deltaTime;
        if (!inAir && velocity.y < 0)
        {
            velocity.y = -2f;
        }
    }
    void JumpLandingTimer()
    {
        if (Physics.Raycast(this.transform.position, -Vector3.up, 2, groundMask) && velocity.y < 0f)
        {
            Debug.Log("landing");
            anim.SetBool("Landing", true);
            landing = true;
            timer = 0.34f;
        }
    }

    void NeutralStateTimer()
    {
        if (timer <= 0)
        {
            anim.SetBool("Landing", false);
            landing = false;
        }
        timer -= Time.deltaTime;
    }

    void ChangeRotation()
    {
        if (velocity.x < 0)
        {
            model.localScale = new Vector3(1f, 1f, -1f);
        }
        if (velocity.x > 0)
        {
            model.localScale = new Vector3(1f, 1f, 1f);
        }
    }

    public void AddVelocity(Vector3 v)
    {
        additionalV += v;
    }

    public void HandleAdditionalV()
    {
        if (Mathf.Abs(additionalV.magnitude) <= (Vector3.one * 5f * Time.deltaTime).magnitude)
		{
            additionalV = Vector3.zero;
		}
		else
		{
            additionalV -= additionalV.normalized * 5f * Time.deltaTime;
		}
        
	}
}