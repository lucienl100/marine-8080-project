
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallJump : MonoBehaviour
{
	public Vector3 lastWallJump;
	public Vector3 collisionLocation;
	Movement movementScript;
	public int groundLayer = 8;
	public float jumpWindow = 0.1f;
	public float jumpTimer = 0f;
	public float minJumpDistance = 2f;
	public float pushStrength = 1f;
	private void Awake()
	{
		movementScript = transform.parent.GetComponent<Movement>();
	}
	// Update is called once per frame
	void Update()
	{
		jumpTimer = Mathf.Max(jumpTimer - Time.deltaTime, 0);
		if (!movementScript.inAir)
		{
			lastWallJump.y = float.PositiveInfinity;
		}
		if (CheckWallAvaliable())
		{
			WallSlide();
			if (Input.GetKeyDown(KeyCode.Space))
			{
				//If the player isn't above where he last wall jumped off on the same wall or is a different wall, allow wall jump
				if (lastWallJump == null || Mathf.Abs(transform.position.x - lastWallJump.x) >= minJumpDistance || lastWallJump.y > transform.position.y)
				{
					bool isRight = collisionLocation.x > transform.position.x ? true : false;
					movementScript.isWallSliding = false;
					movementScript.CeaseControl();
					lastWallJump = transform.position;
					movementScript.Jump();
					movementScript.maxRestrictSpeedScale = 0.4f;
					movementScript.recoverDuration = 3f;
					movementScript.AddVelocity(new Vector3(1, 0, 0) * pushStrength * (isRight ? -1 : 1));
				}
				
			}
		}
		else
		{
			movementScript.isWallSliding = false;
		}
		
	}

	private void OnCollisionStay(Collision collision)
	{
		//Checks if the player is touching a wall
		if (collision.gameObject.layer == groundLayer)
		{
			//Set the timer window for when the player can walljump
			collisionLocation = collision.contacts[0].point;
			jumpTimer = jumpWindow;
		}
	}
	public bool CheckWallAvaliable()
	{
		//Uses the idea that the jumpTimer is always above 0 when colliding with a wall
		return movementScript.inAir && jumpTimer > 0;
	}
	public void WallSlide()
	{
		//Method to check if the player is holding the moving against the wall and change the y velocity in Movement
		bool isRight = collisionLocation.x > transform.position.x ? true : false;
		if ((lastWallJump == null || Mathf.Abs(transform.position.x - lastWallJump.x) >= minJumpDistance || transform.position.y < lastWallJump.y) && ((Input.GetKey(KeyCode.D) && isRight) || (Input.GetKey(KeyCode.A) && !isRight)) && movementScript.velocity.y < 0f)
		{

			movementScript.isWallSliding = true;
			Debug.Log("wallsliding");
		}
		else
		{
			movementScript.isWallSliding = false;
		}
	}
}