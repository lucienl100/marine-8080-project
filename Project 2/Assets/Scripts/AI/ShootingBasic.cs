using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingBasic : MonoBehaviour
{
    public LookAtPlayer lp;
    public GameObject projectile;
    public AudioSource se;
    public float timeBetweenShots = 2f;
    public float intDelay = 0.5f;
    private float timer;
    Transform player;
    Transform t;
    void Start()
    {
        t = this.transform;
        player = lp.player;
        timer = 0f;
    }
    // Update is called once per frame
    void Update()
    {
        if (lp.inRange)
        {
            Debug.Log("fire");
            if (ReadyToFire())
            {
                
                FireProjectile(projectile);
            }
        }
        else
        {
            timer = intDelay;
        }
    }
    public void FireProjectile(GameObject projectile)
    {
        se.Play();
        Vector3 lookdir = player.position - t.position;
        Quaternion dir = Quaternion.LookRotation(lookdir);
        dir.eulerAngles = new Vector3(dir.eulerAngles.x, lookdir.x > 0 ? 90f : 270f, 0f);
        GameObject proj = Instantiate(projectile, t.position, dir);
        proj.GetComponent<BasicProjectile>().player = player;
    }
    public bool ReadyToFire()
    {
        if (timer <= 0)
        {
            timer = timeBetweenShots;
            return true;
        }
        else
        {
            timer -= Time.deltaTime;
            return false;
        }
    }
}
