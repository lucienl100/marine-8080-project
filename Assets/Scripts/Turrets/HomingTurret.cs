using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingTurret : MonoBehaviour, ITurret
{
    public Transform player;
    public GameObject projectile;
    public LayerMask playerLayer;
    public AudioSource sfx;
    private Transform t;
    public float range = 10f;
    public float intFireDelay = 2f;
    private float timeToFire;
    private float firingDelay = 3f;
    Vector3 secondaryFireOff = new Vector3(0f, 0.5f, 0f);
    private float barrelLength = 1.5f;
    public Transform shootingOrigin;
    Vector3 lineOfSightBot;
    float highAngle = 45f;
    void Start() 
    {
        timeToFire = intFireDelay;
        t = this.transform;
        lineOfSightBot = t.forward;
    }
    void Update()
    {
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
        //Called when the player is found by SearchPlayer()
        if (timeToFire <= 0f)
        {
            FireProjectile(projectile);
            timeToFire = firingDelay;
        }
        else
        {
            timeToFire -= Time.deltaTime;
        }
        
    }
    public void RotateTowardsPlayer()
    {
        Vector3 turretToPlayer = player.position - t.position;

        Quaternion turretToPlayerAngle = Quaternion.LookRotation(turretToPlayer);
        t.rotation = Quaternion.Slerp(t.rotation, turretToPlayerAngle, Time.deltaTime * 5f);
    }
    public void FireProjectile(GameObject projectile)
    {
        //Fire two projectiles
        sfx.Play();
        //Projectile upper
        GameObject proju = Instantiate(projectile, t.position + secondaryFireOff + t.forward * barrelLength, t.rotation);
        //Projectile lower
        GameObject projd = Instantiate(projectile, t.position + t.forward * barrelLength, t.rotation);

        //Assign variables to ProjectileHoming
        ProjectileHoming homing = proju.GetComponent<ProjectileHoming>();
        homing.player = player;
        homing.yRotation = t.eulerAngles.y;
        homing = projd.GetComponent<ProjectileHoming>();
        homing.player = player;
        homing.yRotation = t.eulerAngles.y;
    }
    public bool SearchPlayer()
    {
        //Look for player
        Vector3 turretToPlayer = player.position - shootingOrigin.position;
        RaycastHit hit;
        if (CheckHeight() && Vector3.Angle(turretToPlayer, lineOfSightBot) <= highAngle && turretToPlayer.magnitude <= range && Physics.Raycast(shootingOrigin.position, turretToPlayer, out hit, Mathf.Infinity, playerLayer))
        {
            //If the player is in the angle range of the barrel and has a direct line of sight, return true.
            if (hit.collider.tag == "Player")
            {
                return true;
            }
        }
        return false;
    }
    bool CheckHeight()
    {
        //Check if the turret is upside down or not, then checks if the player is below or above the turret barrel respectively
        if (t.parent.rotation.eulerAngles.z == 180f)
        {
            if (player.position.y <= t.position.y)
            {
                return true;
            }
        }
        else
        {
            if (player.position.y >= t.position.y)
            {
                return true;
            }
        }
        return false;
    }
}
