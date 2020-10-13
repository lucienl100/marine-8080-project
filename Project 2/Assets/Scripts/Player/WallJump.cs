﻿
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
	public float pushStrength = 3f;
	// Start is called before the first frame update
	void Start()
	{
	}

	private void Awake()
	{
		movementScript = transform.parent.GetComponent<Movement>();
	}
	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.J))
		{
			movementScript.AddVelocity(new Vector3(3, 0, 0));
		}
		jumpTimer = Mathf.Max(jumpTimer - Time.deltaTime, 0);
		if (!movementScript.inAir)
		{
			lastWallJump.x = float.PositiveInfinity;
		}
		if (movementScript.inAir && jumpTimer > 0 && Input.GetKeyDown(KeyCode.Space))
		{
			if (lastWallJump == null || Mathf.Abs(transform.position.x - lastWallJump.x) >= minJumpDistance)
			{
				lastWallJump = transform.position;
				movementScript.Jump();
				movementScript.AddVelocity(new Vector3(1, 0, 0) * pushStrength * (collisionLocation.x > transform.position.x ? -1 : 1));
			}
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
}