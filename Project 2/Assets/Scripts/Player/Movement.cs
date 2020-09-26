using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public Vector3 velocity;
    public Vector3 acceleration;
    [Range(0, 50)] public float speed;
    public float jumpSpeed;
    public float gravity = -9.81f;
    public bool inAir;
    public bool inJump;
    public CharacterController cc;
<<<<<<< Updated upstream
    public Transform groundCheck;
    public Vector3 groundCheckPosition;
    public LayerMask groundMask;
    float playerHeight = 1.84f;
=======
    public Animator anim;
    public Transform groundCheck;   
    public Transform model;
    public Vector3 groundCheckPosition;
    public LayerMask groundMask;
    float playerHeight = 1.84f;

    float jumptime = 0.8f;
    bool initialJump;
    bool landing = false;
    float timer;
>>>>>>> Stashed changes
    // Start is called before the first frame update
    void Start()
    {
        velocity = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        CalculateAirborne();
        Move();
        CalculateGravity();
        cc.Move(velocity * Time.deltaTime);
        groundCheckPosition = groundCheck.position;
    }

    void Move()
    {
        float x = Input.GetAxis("Horizontal");
        velocity.x = x * speed;
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
<<<<<<< Updated upstream
        if (!inAir)
        {
            velocity.y = jumpSpeed;
        }
=======
        //  Prevents height from decaying if it is not the first jump
        initialJump = false;
        timer = jumptime;
        velocity.y = jumpSpeed;
        inJump = true;
>>>>>>> Stashed changes
	}

    void CalculateAirborne()
	{
        
        if(Physics.CheckSphere(groundCheck.position, 0.5f, groundMask)){
            inAir = false;
            inJump = false;
            initialJump = false;
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
            velocity.y = -4f;
		}
	}
}
