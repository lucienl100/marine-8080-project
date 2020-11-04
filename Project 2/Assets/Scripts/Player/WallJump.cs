
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
			lastWallJump.x = float.PositiveInfinity;
		}
		if (CheckWall())
		{
			WallSlide();
			if (Input.GetKeyDown(KeyCode.Space))
			{
				bool isRight = collisionLocation.x > transform.position.x ? true : false;
				if (lastWallJump == null || Mathf.Abs(transform.position.x - lastWallJump.x) >= minJumpDistance)
				{
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
			movementScript.gravityScale = 1f;
		}
		
	}

	private void OnCollisionStay(Collision collision)
	{
		if (collision.gameObject.layer == groundLayer)
		{
			collisionLocation = collision.contacts[0].point;
			jumpTimer = jumpWindow;
		}
	}
	public bool CheckWall()
	{
		return movementScript.inAir && jumpTimer > 0;
	}
	public void WallSlide()
	{
		bool isRight = collisionLocation.x > transform.position.x ? true : false;
		if (CheckWall() && (lastWallJump == null || Mathf.Abs(transform.position.x - lastWallJump.x) >= minJumpDistance) && ((Input.GetKey(KeyCode.D) && isRight) || (Input.GetKey(KeyCode.A) && !isRight)) && movementScript.velocity.y < 0f)
		{

			movementScript.gravityScale = 0.25f;
			Debug.Log("wallsliding");
		}
		else
		{
			movementScript.gravityScale = 1f;
		}
	}
}