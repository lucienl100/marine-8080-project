using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallJump : MonoBehaviour
{
    public CapsuleCollider cc;
    public Vector3 lastWallJump;
    Movement movementScript;
    public int groundLayer = 8;
	public float jumpWindow = 0.1f;
	public float jumpTimer  = 0f;
	public float minJumpDistance = 2f;
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
		jumpTimer = Mathf.Max(jumpTimer - Time.deltaTime, 0);
		if (!movementScript.inAir)
		{
			lastWallJump.x = float.PositiveInfinity;
		}
		if (jumpTimer > 0 && Input.GetKeyDown(KeyCode.Space))
		{
			if (lastWallJump == null || Mathf.Abs(transform.position.x - lastWallJump.x) >= minJumpDistance)
			{
				lastWallJump = transform.position;
				movementScript.Jump();
			}
		}
    }

	private void OnTriggerStay(Collider other)
	{
		if(other.gameObject.layer == groundLayer)
		{
			Debug.Log("Wall");
		}
		if(other.gameObject.layer == groundLayer)
		{
			jumpTimer = jumpWindow;
		}
	}
	private void OnCollisionEnter(Collision collision)
	{
		Debug.Log("Different Collsion");
	}
	void OnCollisionStay(Collision collision)
	{
		Debug.Log("Collision");
		if(collision.gameObject.layer == groundLayer && Input.GetKeyDown(KeyCode.Space))
		{
            movementScript.Jump();
		}
	}
}
