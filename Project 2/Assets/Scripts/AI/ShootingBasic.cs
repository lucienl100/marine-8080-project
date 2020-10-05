using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingBasic : MonoBehaviour
{
    public LookAtPlayer lp;
    public GameObject projectile;
    public AudioSource se;
    public float timeBetweenShots = 2f;
    private float timer;
    public Transform player;
    Transform t;
    void Start()
    {
        timer = timeBetweenShots;
        t = this.transform;
    }
    // Update is called once per frame
    void Update()
    {
        if (lp.inRange)
        {
            if (ReadyToFire())
            {
                FireProjectile(projectile);
            }
        }
        else
        {
            timer = timeBetweenShots;
        }
    }
    public void FireProjectile(GameObject projectile)
    {
        se.Play();
        Vector3 lookdir = player.position - t.position;
        Quaternion dir = Quaternion.LookRotation(lookdir);
        dir.eulerAngles = new Vector3(dir.eulerAngles.x, lookdir.x > 0 ? 90f : 270f, 0f);
        Instantiate(projectile, t.position, dir);
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
