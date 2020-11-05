using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenetrationProjectile : MonoBehaviour
{
    public float damage = 10f;
    public float speed = 5f;
    public float maxDist = 50f;
    Vector3 startPosition;
    private Transform t;
    public GameObject explosion;
    public Transform player;
    bool collided = false;
    public Quaternion rotation;
    Vector3 dest;
    void Start()
    {
        t = this.transform;
        startPosition = t.position;
        rotation = t.rotation;
        Debug.Log(rotation.eulerAngles);
        dest = t.position + t.rotation * Vector3.forward * maxDist;
    }
    void Update()
    {
        OutOfBoundsCheck();
        Fly();
    }
    public void Fly()
    {
        //Lock the z position
        t.position =  new Vector3(Mathf.Lerp(t.position.x, dest.x, Time.deltaTime * speed), t.position.y, -2.5f);
        t.rotation = rotation;
    }
    public void OnTriggerEnter(Collider c)
    {
        Debug.Log("Collided!");
        if (c.gameObject.tag == "Player" && collided == false)
        {
            //Damage the player
            player.GetComponent<PlayerHealth>().Damage(damage);
            GameObject ex = Instantiate(explosion, t.position, t.rotation);
            Destroy(ex, 0.5f);
            collided = true;
        }
        else if (c.gameObject.tag != "Projectile" && c.gameObject.tag != "Enemy" && c.gameObject.tag != "IgnoreProjectiles" && c.gameObject.tag != "Player")
        {
            //Destroy the projectile if it hits a shield or ground
            GameObject ex = Instantiate(explosion, t.position, t.rotation);
            Destroy(this.gameObject);
            Destroy(ex, 0.5f);
           
        }
    }
    public void OutOfBoundsCheck()
    {
        //Destroy the projectile if it flies too far from the start position
        if ((t.position - startPosition).magnitude > maxDist - 1f)
        {
            Destroy(this.gameObject);
        }
    }
}
