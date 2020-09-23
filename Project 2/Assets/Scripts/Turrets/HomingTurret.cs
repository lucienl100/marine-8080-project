using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingTurret : MonoBehaviour, ITurret
{
    public Transform player;
    public GameObject projectile;
    private Transform t;
    public float range = 10f;
    public float intFireDelay = 2f;
    private float timeToFire;
    private float firingDelay = 3f;
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
    public void FireProjectile(GameObject projectile)
    {
        Vector3 turretToPlayer = player.position - t.position;
        
        Quaternion turretToPlayerAngle = Quaternion.LookRotation(turretToPlayer);
        GameObject proj = Instantiate(projectile, t.position + turretToPlayer.normalized, turretToPlayerAngle);
        ProjectileHoming homing = proj.GetComponent<ProjectileHoming>();
        homing.player = player;
        homing.yRotation = t.eulerAngles.y;
    }
    public bool SearchPlayer()
    {
        Vector3 turretToPlayer = player.position - t.position;
        if (player.position.y >= t.position.y && Vector3.Angle(turretToPlayer, lineOfSightBot) <= highAngle && turretToPlayer.magnitude <= range)
        {
            return true;
        }
        return false;
    }
}
