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
    public CharacterController cc;
    public Transform groundCheck;
    public Vector3 groundCheckPosition;
    public LayerMask groundMask;
    float playerHeight = 1.84f;
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
            velocity.y = jumpSpeed;
        }
	}
    void CalculateAirborne()
	{
        
        if(Physics.CheckSphere(groundCheck.position, 0.5f, groundMask)){
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
            velocity.y = -4f;
		}
	}
}
