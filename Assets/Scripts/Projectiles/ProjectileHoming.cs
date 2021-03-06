﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileHoming : MonoBehaviour, IProjectile
{
    public float speed = 1f;
    public float damage = 20f;
    public float intSpeed = 1f;
    public float maxSpeed = 10.0f;
    public float acceleration = 5f;
    public float rotateSpeed = 25f;
    public float yRotation = 270f;
    public GameObject explosion;
    public GameObject emitterFire;
    public GameObject emitterSmoke;
    private Vector3 finalLookRotation;
    public bool facingRight = false;
    private Transform t;
    public Transform player;
    private float errorMargin = 0.1f;
    private float timerDuration = 0.05f;
    private float timer;
    private float depth = -2.5f;
    void Start() 
    {
        timer = timerDuration;
        if (facingRight)
        {
            yRotation = 90f; //Facing right
        }
        t = this.transform;
    }
    void Update()
    {
        //Speed up over time
        speed += Mathf.Clamp(Mathf.Exp(2f*speed)-1f, maxSpeed, intSpeed) * Time.deltaTime * acceleration;
        RotateTowardsPlayer();
        Fly();
    }
    public void RotateTowardsPlayer()
    {
        Vector3 lookDir = player.position - t.position;
        lookDir.z = 0f;
        //Get the rotation between the current angle and the angle towards the player
        t.rotation = Quaternion.Lerp(t.rotation, Quaternion.FromToRotation(Vector3.forward, lookDir), Time.deltaTime * 3f);
        t.eulerAngles = new Vector3(t.rotation.eulerAngles.x, yRotation, 0f);
    }
    public void Fly()
    {
        t.position += t.forward * speed * Time.deltaTime;
        //Account for rotation error
        t.position = new Vector3(t.position.x, t.position.y, depth);
    }
    public void OnTriggerEnter(Collider c) 
    {
        
        if (c.gameObject.tag == "Player")
        {
            Debug.Log("Collided!");
            player.GetComponent<PlayerHealth>().Damage(damage);
            Explode();
        }
        else if (c.gameObject.tag != "Projectile" && c.gameObject.tag != "Enemy")
        {
            Explode();
        }
    }
    public void Explode()
    {
        emitterFire.transform.parent = null;
        emitterSmoke.transform.parent = null;
        emitterFire.GetComponent<Expiry>().timeLeft = 1.5f;
        emitterSmoke.GetComponent<Expiry>().timeLeft = 1.5f;
        emitterFire.GetComponent<ParticleSystem>().Stop();
        emitterSmoke.GetComponent<ParticleSystem>().Stop();
        GameObject ex = Instantiate(explosion, t.position, t.rotation);
        Destroy(ex, 0.25f);
        Destroy(this.gameObject);
    }
}
