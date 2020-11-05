using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class R2Turret : MonoBehaviour
{
    public Transform player;
    public GameObject projectile;
    public AudioSource sfx;
    private Transform t;
    public LayerMask playerLayer;
    public float range = 15f;
    public float intFireDelay = 0.15f;
    private float timeToFire;
    private float firingDelay = 1.5f;
    Sliding playerslide;
    public Transform firingPosition;

    Vector3 dirToLook;
    Vector3 slideDirToLook;
    void Start()
    {
        playerslide = player.gameObject.GetComponent<Sliding>();
        timeToFire = intFireDelay;
        t = this.transform;
    }
    void Update()
    {
        //If player is found by the turret
        if (SearchPlayer())
        {
            RotateTowardsPlayer();
            ArmTurret();
        }
        else
        {
            timeToFire = intFireDelay;
        }
    }
    public void ArmTurret()
    {
        if (timeToFire <= 0f)
        {
            FireProjectile(projectile);
            timeToFire = firingDelay;
        }
        timeToFire -= Time.deltaTime;
    }
    public void RotateTowardsPlayer()
    {
        Vector3 turretToPlayer = playerslide.isSliding ? slideDirToLook : dirToLook;
        Quaternion turretToPlayerAngle = Quaternion.LookRotation(turretToPlayer);
        t.rotation = Quaternion.Slerp(t.rotation, turretToPlayerAngle, Time.deltaTime * 10f);
    }
    public void FireProjectile(GameObject projectile)
    {
        //Fire the projectile
        sfx.Play();
        GameObject proj = Instantiate(projectile, firingPosition.position, t.rotation);
        BasicProjectile bp = proj.GetComponent<BasicProjectile>();

        //Assign player variable to BasicProjectile script
        bp.player = player;
    }
    public bool SearchPlayer()
    {
        Vector3 turretToPlayer;

        //Account for sliding hitbox depending on if the player is sliding or not
        dirToLook = player.position - t.position;
        slideDirToLook = new Vector3(player.position.x, player.position.y - 1f, player.position.z) - t.position;
        if (!playerslide.isSliding)
        {
            turretToPlayer = dirToLook;
        }
        else
        {
            turretToPlayer = slideDirToLook;
        }
        RaycastHit hit;
        //Try to detect player through raycast
        if (CheckHeight() && turretToPlayer.magnitude <= range && Physics.Raycast(t.position, turretToPlayer, out hit, Mathf.Infinity, playerLayer))
        {
            if (hit.collider.tag == "Player")
            {
                return true;
            }
        }
        return false;
    }
    bool CheckHeight()
    {
        //Check if the barrel can rotate towards the player
        if (t.parent.rotation.eulerAngles.z == 180f)
        {
            if (player.position.y <= t.position.y)
            {
                return true;
            }
        }
        else if (t.parent.rotation.eulerAngles.z == 0f && t.parent.rotation.eulerAngles.y == 270f)
        {
            if (player.position.y >= t.position.y)
            {
                return true;
            }
        }
        else if (t.parent.rotation.eulerAngles.x == 90f)
        {
            if (player.position.x >= t.position.x)
            {
                return true;
            }
        }
        else if (t.parent.rotation.eulerAngles.x == 270f)
        {
            if (player.position.x >= t.position.x)
            {
                return true;
            }
        }
        return false;
    }
}
