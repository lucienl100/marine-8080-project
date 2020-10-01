using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicProjectile : MonoBehaviour
{
    public float speed = 5f;
    public float maxDist = 50f;
    Vector3 startPosition;
    private Transform t;
    public Transform player;
    public Quaternion rotation;
    void Start()
    {
        t = this.transform;
        startPosition = t.position;
        rotation = t.rotation;
    }
    void Update()
    {
        OutOfBoundsCheck();
        Fly();
    }
    public void Fly()
    {
        t.position += t.forward * Time.deltaTime * speed;
        t.rotation = rotation;
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
            Destroy(this.gameObject);
        }
        else if (c.gameObject.tag != "Projectile" && c.gameObject.tag != "Enemy")
        {
            Destroy(this.gameObject);
        }
    }
    public void OutOfBoundsCheck()
    {
        if ((t.position - startPosition).magnitude > maxDist)
        {
            Destroy(this.gameObject);
        }
    }
}
