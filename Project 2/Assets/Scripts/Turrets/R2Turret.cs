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
    public Transform firingPosition;
    void Start()
    {
        timeToFire = intFireDelay;
        t = this.transform;
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
        if (timeToFire <= 0f)
        {
            FireProjectile(projectile);
            timeToFire = firingDelay;
        }
        timeToFire -= Time.deltaTime;
    }
    public void RotateTowardsPlayer()
    {
        Vector3 turretToPlayer = player.position - t.position;

        Quaternion turretToPlayerAngle = Quaternion.LookRotation(turretToPlayer);
        t.rotation = Quaternion.Slerp(t.rotation, turretToPlayerAngle, Time.deltaTime * 10f);
    }
    public void FireProjectile(GameObject projectile)
    {
        sfx.Play();
        GameObject proj = Instantiate(projectile, firingPosition.position, t.rotation);
        BasicProjectile bp = proj.GetComponent<BasicProjectile>();
        bp.player = player;
    }
    public bool SearchPlayer()
    {
        Vector3 turretToPlayer = player.position - t.position;
        RaycastHit hit;
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
