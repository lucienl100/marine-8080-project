using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class Movement : MonoBehaviour
{
    public Vector3 velocity;
    public Vector3 acceleration;
    [Range(0, 50)] public float speed;
    public float jumpSpeed;
    public float gravity = -9.81f;
    public bool inAir;
    public CharacterController cc;
    public Animator anim;
    public Transform groundCheck;
    public Transform model;
    public Vector3 groundCheckPosition;
    public LayerMask groundMask;
    float playerHeight = 1.84f;

    float jumptime = 0.8f;
    bool landing = false;
    float timer;
    // Start is called before the first frame update
    void Start()
    {
        timer = 0f;
        velocity = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        CalculateAirborne();
        Move();
        CalculateGravity();
        //Animation methods
        cc.Move(velocity * Time.deltaTime);
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
        velocity.x = x * speed;
		if (Input.GetKeyDown(KeyCode.Space))
		{
            Jump();
		}
        velocity.z = 0;
        
    }

    void Jump()
	{
        if (!inAir)
        {
            timer = jumptime;
            velocity.y = jumpSpeed;
        }
	}
    void CalculateAirborne()
	{
        
        if(Physics.CheckSphere(groundCheck.position, 0.25f, groundMask)){
            inAir = false;
		}
		else
		{
            inAir = true;
		}
	}

    void CalculateGravity()
	{
        velocity.y += gravity * Time.deltaTime;
        if(!inAir && velocity.y < 0)
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
}
