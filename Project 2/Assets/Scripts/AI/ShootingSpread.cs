using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingSpread : MonoBehaviour
{
    public LookAtPlayer lp;
    public GameObject projectile;
    public AudioSource se;
    public float timeBetweenShots = 2f;
    public float intDelay = 0f;
    private float timer;
    Transform player;
    public Transform shootingOrigin;
    void Start()
    {
        player = lp.player;
        timer = 0f;
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
            timer = intDelay;
        }
    }
    public void FireProjectile(GameObject projectile)
    {
        se.Play();
        Vector3 lookdir = (player.position - shootingOrigin.position).normalized;
        
        //Make sure the projectiles fire straight in the x axis.
        
        for (int i = 0; i < 4; i++)
        {
            Vector3 newDir = new Vector3(lookdir.x, lookdir.y + (float)(i - 2) * 0.2f, lookdir.z);
            Quaternion dir = Quaternion.LookRotation(newDir);
            dir.eulerAngles = new Vector3(dir.eulerAngles.x, newDir.x > 0 ? 90f : 270f, 0f);
            GameObject proj = Instantiate(projectile, shootingOrigin.position, dir);
            //Set the player attribute of the projectile to player to destroy it when out of range.

            proj.GetComponent<BasicProjectile>().player = player;
        }
        
    }
    public bool ReadyToFire()
    //Method to tick a timer and return true when timer hits zero.
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
