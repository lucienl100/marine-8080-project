using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailgunProjectile : MonoBehaviour, IProjectile
{
    public float speed = 300f;
    public float maxDist = 300f;
    public float maxPlayerDist = 300f;
    public float lifetime = 2f;
    private float timer = 2f;
    public ParticleSystem emit1;
    public ParticleSystem emit2;
    private Transform t;
    private Vector3 startPosition;
    private Vector3 rotation;
    public Transform player;
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
        if (CheckPlayer())
        {
            DamagePlayer();
        }
        t.position += t.forward * Time.deltaTime * speed;
    }
    public bool CheckPlayer()
    {
        RaycastHit hit;
        float endCheck = Mathf.Min((t.position - startPosition).magnitude, maxDist);
        float dist = (player.position - t.position).magnitude;
        if (Physics.Raycast(t.position, -rotation, out hit) && dist <= endCheck && timer >= 1f)
        {
            if (hit.collider.gameObject.tag == "Player")
            {
                Debug.Log("Railgun hit player!");
                DamagePlayer();
            }
            
        }
        return false;
    }
    public void DamagePlayer()
    {
        //Damage the player
    }
    public void OnCollisionEnter(Collision c) 
    {
        Debug.Log("Collided!");
        if (c.gameObject.tag == "Player")
        {
            //Damage the player
        }
    }
    public void TimerCheck()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            emit1.transform.parent = null;
            emit2.transform.parent = null;
            Destroy(this.gameObject);
        }
    }
    public void OutOfBoundsCheck()
    {
        float dist = (t.position - player.position).magnitude;
        if (dist > maxPlayerDist)
        {
            speed = 0f;
        }
        if ((t.position - startPosition).magnitude > maxDist)
        {
            speed = 0f;
        }
    }
}
