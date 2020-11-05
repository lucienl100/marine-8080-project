using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMovement : MonoBehaviour
{
    // Update is called once per frame
    Quaternion faceRotation = Quaternion.Euler(new Vector3(0f, 90f, 0f));
    public Transform player;
    public Animator anim;
    public float minRange = 3f;
    public bool playerIsRight;
    public bool cd = false;
    public bool stopRotation = false;
    Vector3 dirToLook;
    Transform t;
    public Transform chest;
    void Start()
    {
        t = this.transform;
        Physics.IgnoreCollision(player.GetComponent<Collider>(), GetComponent<Collider>());
        playerIsRight = (player.position.x - t.position.x) > 0 ? true : false;
    }
    void Update()
    {
        if (cd == false)
        {
            dirToLook = player.position - t.position;
            CheckRotation();
            FollowPlayer();
        }
    }
    void LateUpdate()
    {
        if (stopRotation)
        {
            Quaternion rotation = Quaternion.LookRotation(dirToLook);
            rotation.eulerAngles = new Vector3(0f, rotation.eulerAngles.y + 90f, rotation.eulerAngles.x);
            chest.rotation = rotation;
        }
    }
    void FollowPlayer()
    {
        //Follow the player until within a minimum range
        if (Mathf.Abs(player.position.x - t.position.x) > minRange)
        {
            anim.SetBool("Following", true);
            t.position += new Vector3(t.forward.x, 0f, 0f) * Time.deltaTime * 3f;
            t.position = new Vector3(t.position.x, t.position.y, -2.5f);
        }
        else
        {
            anim.SetBool("Following", false);
        }
    }
    void CheckRotation()
    {
        //Face player
        if (t.position.x < player.position.x - 0.1f)
        {
            if (!playerIsRight)
            {
                playerIsRight = true;
            }
            faceRotation = Quaternion.Euler(new Vector3(0f, 90f, 0f));

        }
        else if (t.position.x > player.position.x + 0.1f)
        {
            faceRotation = Quaternion.Euler(new Vector3(0f, 270f, 0f));
            if (playerIsRight)
            {
                playerIsRight = false;
            }
        }
        t.rotation = Quaternion.Slerp(t.rotation, faceRotation, Time.deltaTime * 15.0f);
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
}
