using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileHoming : MonoBehaviour, IProjectile
{
    public float speed = 1f;
    public float intSpeed = 1f;
    public float maxSpeed = 10.0f;
    public float acceleration = 5f;
    public float rotateSpeed = 25f;
    public float yRotation = 270f;
    private Vector3 finalLookRotation;
    public bool facingRight = false;
    private Transform t;
    public Transform player;
    void Start() 
    {
        if (facingRight)
        {
            yRotation = 90f;
        }
        t = this.transform;
    }
    void Update()
    {
        speed += Mathf.Clamp(Mathf.Exp(2f*speed)-1f, maxSpeed, intSpeed) * Time.deltaTime * acceleration;
        Debug.Log(speed);
        if (yRotation == 270f && player.position.x - t.position.x < 0 || yRotation == 90f && player.position.x - t.position.x > 0)
        {
            RotateTowardsPlayer();
        }
        Fly();
    }
    public void RotateTowardsPlayer()
    {
        Vector3 lookDir = player.position - t.position;
        Quaternion toPlayer = Quaternion.LookRotation(lookDir);
        
        Quaternion lookRotation = Quaternion.RotateTowards(t.localRotation, toPlayer, Time.deltaTime * rotateSpeed);
        t.rotation = lookRotation;
        t.eulerAngles = new Vector3(t.eulerAngles.x, yRotation, 0f);
    }
    public void Fly()
    {
        t.position += t.forward * speed * Time.deltaTime;
    }
    public void OnCollisionEnter(Collision c) 
    {
        Debug.Log("Collided!");
        if (c.gameObject.tag == "Player")
        {
            //Damage the player
            Destroy(this.gameObject);
        }
        else if (c.gameObject.tag != "Projectile")
        {
            Destroy(this.gameObject);
        }
    }
}
