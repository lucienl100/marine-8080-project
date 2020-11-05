using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Railgun : MonoBehaviour, ITurret
{
    public bool upsideDown = false;
    public AudioSource se;
    public Transform player;
    public GameObject tracer;
    public float damage = 50f;
    public GameObject rail;
    public LayerMask groundLayer;
    private Transform t;
    private Vector3 adjustedBase;
    private Vector3 barrelEnd;
    private bool isLocked = false;
    private bool readyToFire = false;
    private Vector3 turretToPlayer;
    private Vector3 currentRotation;
    private float facingRight = -1f;
    public float rotateSpeed = 20f;
    public float range = 40f;
    public float maxDistance = 300f;
    private float distToCollision;
    public float intFireDelay = 1f;
    private float unarmDelay = 0.5f;
    private float barrelLength = 3.5f;
    private float delayBetweenWarning = 1.5f;
    private float timeToFireTracer;
    private float timeToFireRail;
    private float timeToUnarm;
    public float firingDelay = 2.5f;
    public LayerMask playerLayer;
    Vector3 lineOfSightBot;
    float highAngle = 45f;
    //public GameObject hitbox;
    void Start() 
    {
        t = this.transform;
        adjustedBase = new Vector3(t.position.x, t.position.y - 0.05f, t.position.z);
        timeToFireTracer = intFireDelay;
        timeToFireRail = delayBetweenWarning;
        timeToUnarm = 0f;
        barrelEnd = adjustedBase - new Vector3(0.05f, 0f, 0f) + facingRight * new Vector3(barrelLength, 0f, 0f);
        lineOfSightBot = t.forward;
    }
    void Update()
    {
        turretToPlayer = player.position - t.position;
        if (timeToUnarm > 0f)
        {
            //Allow movement of turret barrel
            Unarm();
        } 
        else if (SearchPlayer() && !readyToFire)
        {
            //If player is found, tick timer to shoot
            if (!isLocked)
            {
                RotateBarrel();
            }
            ArmTurret();
        }
        else if (readyToFire)
        {
            ArmRailgun();
        }
        else
        {
            timeToFireTracer = intFireDelay;
            timeToFireRail = delayBetweenWarning;
        }
    }
    public void RotateBarrel()
    {
        Quaternion angleToPlayer = Quaternion.LookRotation(turretToPlayer);
        t.rotation = Quaternion.RotateTowards(t.rotation, angleToPlayer, Time.deltaTime * rotateSpeed);
    }
    public void ArmTurret()
    {
        if (timeToFireTracer <= 0f)
        {
            currentRotation = t.rotation * Vector3.forward;
            isLocked = true;
            readyToFire = true;
            distToCollision = maxDistance;
            RaycastHit hit;
            
            if (Physics.Raycast(adjustedBase + barrelLength * currentRotation, currentRotation, out hit, Mathf.Infinity, groundLayer))
            {
                //Find the distance travelled by the raycast
                distToCollision = Mathf.Min(maxDistance, hit.distance);
            }
            FireTracer(tracer);
            timeToFireTracer = firingDelay;
        }
        timeToFireTracer -= Time.deltaTime;
    }
    public void FireTracer(GameObject projectile)
    {
        //Fire warning tracer
        GameObject proj = Instantiate(projectile, adjustedBase + barrelLength * currentRotation, t.rotation);
        RailgunTracer tracer = proj.GetComponent<RailgunTracer>();
        tracer.player = player;
        tracer.maxDist = distToCollision;
    }
    public void FireProjectile(GameObject projectile)
    {
        //Fire actual laser visual
        se.Play();
        GameObject proj = Instantiate(projectile, adjustedBase + barrelLength * currentRotation, t.rotation);
        RailgunProjectile rail = proj.GetComponent<RailgunProjectile>();
        Invoke("CheckRayDamage", 0.1f);
        //Set the maxDist of the visual effect to the distance of the raycast in ArmTurret()
        rail.maxDist = distToCollision;
    }
    void CheckRayDamage()
    {
        //Checks for the player in the laser fire and damages player if found
        RaycastHit hit;
        Vector3 barrelposition = adjustedBase + barrelLength * currentRotation;
        if (Physics.Raycast(barrelposition, t.rotation * Vector3.forward, out hit, distToCollision, playerLayer))
        {
            if (hit.collider.tag == "Player")
            {
                player.GetComponent<PlayerHealth>().Damage(damage);
            }
        }
    }
    public bool SearchPlayer()
    {
        RaycastHit hit;
        //Checks the turret barrel can rotate towards the player, the player is in range and there is a straight line of sight
        if (!upsideDown)
        {
            
            if (player.position.y >= barrelEnd.y && Vector3.Angle(turretToPlayer, lineOfSightBot) <= highAngle && turretToPlayer.magnitude <= range && Physics.Raycast(t.position, turretToPlayer, out hit, Mathf.Infinity, playerLayer))
            {
                if (hit.collider.tag == "Player")
                {
                    return true;
                }
            }
        }
        else
        {
            if (player.position.y <= barrelEnd.y && Vector3.Angle(turretToPlayer, lineOfSightBot) <= highAngle && turretToPlayer.magnitude <= range && Physics.Raycast(t.position, turretToPlayer, out hit, Mathf.Infinity, playerLayer))
            {
                if (hit.collider.tag == "Player")
                {
                    return true;
                }
            }
        }
        
        return false;
    }
    public void ArmRailgun()
    {
        if (timeToFireRail <= 0f)
        {
            //Fire railgun and reset timers
            isLocked = false;
            readyToFire = false;
            FireProjectile(rail);
            timeToUnarm = unarmDelay;
            timeToFireRail = delayBetweenWarning;
        }
        timeToFireRail -= Time.deltaTime;
    }
    public void Unarm()
    {
        timeToUnarm -= Time.deltaTime;
    }
}
