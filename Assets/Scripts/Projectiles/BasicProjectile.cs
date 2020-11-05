using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicProjectile : MonoBehaviour, IProjectile
{
    public float damage = 10f;
    public float speed = 5f;
    public float maxDist = 50f;
    Vector3 startPosition;
    private Transform t;
    public GameObject explosion;
    public Transform player;
    public Quaternion rotation;
    public float depth = -2.5f;
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
        t.position = new Vector3(t.position.x, t.position.y, depth);
        t.rotation = rotation;
    }
    public void DamagePlayer()
    {
        //Damage the player
    }
    public void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.tag == "Player")
        {
            
            player.GetComponent<PlayerHealth>().Damage(damage);
            GameObject ex = Instantiate(explosion, t.position, t.rotation);
            Destroy(this.gameObject);
            Destroy(ex, 0.5f);
            
        }
        else if (c.gameObject.tag != "Projectile" && c.gameObject.tag != "Enemy" && c.gameObject.tag != "IgnoreProjectiles")
        {
            GameObject ex = Instantiate(explosion, t.position, t.rotation);
            Destroy(this.gameObject);
            Destroy(ex, 0.5f);
           
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
