using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailgunTracer : MonoBehaviour, IProjectile
{
    public float speed = 300f;
    public float maxDist = 300f;
    public float maxPlayerDist = 300f;
    public float lifetime = 1.5f;
    public float timer = 1.5f;
    private Transform t;
    private Vector3 intPos;
    public Transform player;
    void Start() 
    {
        t = this.transform;
        timer = lifetime;
        intPos = t.position;
    }
    void Update()
    {
        TimerCheck();
        OutOfBoundsCheck();
        Fly();
    }
    public void Fly()
    {
        t.position += t.forward * Time.deltaTime * speed;
    }
    public void TimerCheck()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            Destroy(this.gameObject);
        }
    }
    public void OutOfBoundsCheck()
    {
        float dist = (t.position - player.position).magnitude;
        if (dist > maxPlayerDist)
        {
            Destroy(this.gameObject);
        }
        if ((t.position - intPos).magnitude > maxDist)
        {
            speed = 0f;
        }
    }
}
