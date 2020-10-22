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
    public GameObject emitterFire;
    public GameObject emitterSmoke;
    private Vector3 finalLookRotation;
    public bool facingRight = false;
    private Transform t;
    public Transform player;
    private float errorMargin = 0.1f;
    private float timerDuration = 0.05f;
    private float timer;
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
        speed += Mathf.Clamp(Mathf.Exp(2f*speed)-1f, maxSpeed, intSpeed) * Time.deltaTime * acceleration;
        RotateTowardsPlayer();
        Fly();
    }
    public void RotateTowardsPlayer()
    {
        Vector3 lookDir = player.position - t.position;
        lookDir.z = 0f;
        //getting the angle between the this -> target and the rigidbody.rotation vector
        t.rotation = Quaternion.Lerp(t.rotation, Quaternion.FromToRotation(Vector3.forward, lookDir), Time.deltaTime * 3f);
        t.eulerAngles = new Vector3(t.rotation.eulerAngles.x, yRotation, 0f);
    }
    public void Fly()
    {
        t.position += t.forward * speed * Time.deltaTime;
    }
    public void OnCollisionEnter(Collision c) 
    {
        
        if (c.gameObject.tag == "Player")
        {
            Debug.Log("Collided!");
            //Damage the player
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
        emitterSmoke.SetActive(false);
        Destroy(this.gameObject);
    }
}
