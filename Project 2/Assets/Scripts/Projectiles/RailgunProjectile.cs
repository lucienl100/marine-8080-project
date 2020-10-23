using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailgunProjectile : MonoBehaviour, IProjectile
{
    public float speed = 500f;
    public float maxDist = 300f;
    public float maxPlayerDist = 300f;
    public float lifetime = 2f;
    private float timer = 2f;
    private Transform t;
    private Vector3 startPosition;
    private Vector3 rotation;
    void Start() 
    {
        t = this.transform;
        timer = lifetime;
        startPosition = t.position;
        rotation = t.rotation * Vector3.forward;
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
        if (timer <= 0f)
        {
            Destroy(this.gameObject);
        }
    }
    public void OutOfBoundsCheck()
    {
        if ((t.position - startPosition).magnitude > maxDist)
        {
            speed = 0f;
        }
    }
}
