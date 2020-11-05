using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    private bool goingUp;
    public float height = 10f;
    public float timerDuration = 3f;
    Transform t;
    Rigidbody rb;
    private Vector3 upper;
    private Vector3 bottom;
    private float timer;
    public float speed = 5f;
    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        t = this.transform;
        bottom = t.position;
        upper = new Vector3(t.position.x, t.position.y + height, t.position.z);
        goingUp = true;
        timer = timerDuration;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer <= 0f)
        {
            Move();
        }
        timer -= Time.deltaTime;
    }
    void Move()
    {
        if (goingUp)
        {
            t.position = Vector3.MoveTowards(t.position, upper, Time.deltaTime * speed);

            if (t.position.y >= upper.y)
            {
                timer = timerDuration;
                goingUp = !goingUp;
            }
        }
        else
        {
            t.position = Vector3.MoveTowards(t.position, bottom, Time.deltaTime * speed);
            if (t.position.y <= bottom.y)
            {
                timer = timerDuration;
                goingUp = !goingUp;
            }
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("collided");
        if (collision.transform.tag == "Ground Check")
        {
            collision.transform.parent.parent = t;
        }
    }
    void OnCollisionExit(Collision collision)
    {
        if (collision.transform.tag == "Ground Check")
        {
            collision.transform.parent.parent = null;
        }
    }
}
