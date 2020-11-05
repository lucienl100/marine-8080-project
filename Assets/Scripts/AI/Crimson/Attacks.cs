using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacks : MonoBehaviour
{
    Transform t;
    public Transform player;
    public Transform headLaserOrigin;
    public Transform sweepHitbox;
    public Transform swipeHitbox;
    public Animator anim;
    public GameObject laser;
    public GameObject laserwarning;
    public GameObject sweepProjectile;
    public float sweepDamage = 15f;
    public LayerMask playerLayer;
    public LayerMask groundPlayer;
    BossMovement mv;
    PlayerHealth pH;
    
    Movement pMV;

    public bool sweep;
    Vector3 hitboxsize = new Vector3(2.5f, 1f, 2.5f);
    Vector3 swipehitbox = new Vector3(4.7f, 3f, 4.7f);
    public float swipeDamage = 39f;
    float sweepForce = 10f;
    
    public ParticleSystem sweepPE;

    public ParticleSystem laserLoadup;
    public ParticleSystem laserExplosion;
    public Quaternion laserAngle;
    public float laserDamage = 50f;
    Vector3 laserOrigin;
    public AudioSource loadupaudio;
    public AudioSource laseraudio;
    Vector3 lookDir;

    private float timer = 3f;

    public ParticleSystem esPE;
    public ParticleSystem ssPE;
    public GameObject spreadShotProj;
    public AudioSource spreadshotSE;

    public AudioSource sweepAudio;
    public AudioSource swipeAudio;
    // Start is called before the first frame update
    void Start()
    {
        mv = this.GetComponent<BossMovement>();
        pH = player.GetComponent<PlayerHealth>();
        pMV = player.GetComponent<Movement>();
        t = this.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer <= 0f)
        {
            PerformMove(RollMove());
        }
        timer -= Time.deltaTime;
     
    }
    float RollMove()
    {
        if (Mathf.Abs(t.position.x - player.position.x) < 2f)
        {
            return Random.Range(0f, 0.49f);
        }
        if (Mathf.Abs(t.position.x - player.position.x) < 10f)
        {
            return Random.Range(0f, 0.75f);
        }
        else
        {
            return Random.Range(0.28f, 1f);
        }
    }
    void PerformMove(float moveF)
    {
        Debug.Log(moveF);
        if (moveF < 0.25f)
        {
            Swipe();
            timer = 2f;
        }
        else if (moveF < 0.5f)
        {
            Sweep();
            timer = 2.5f;
        }
        else if (moveF < 0.75f)
        {
            SuccesiveFire();
            timer = 3.5f;
        }
        else
        {
            Laser();
            timer = 2.5f;
        }
        anim.SetBool("Following", false);
        mv.cd = true;
    }
    void Sweep()
    {
        Invoke("SweepSound", 0.85f);
        anim.SetTrigger("Sweep");
        sweepPE.Play();
        Invoke("SweepDamage", 1.1f);
        Invoke("FireSweep", 1.1f);
        Invoke("SweepRecover", 1.8f);
    }
    void SweepSound()
    {
        sweepAudio.Play();
    }
    void SweepDamage()
    {
        //Applies damage to anyone in range of the sweep, on top of the projectile
        if (Physics.CheckBox(sweepHitbox.position, hitboxsize, Quaternion.identity, playerLayer))
        {
            pH.Damage(sweepDamage);
            pMV.AddVelocity((mv.playerIsRight ? 1f : -1f) * new Vector3(1f, 0f, 0f) * sweepForce);
            pMV.CeaseControl();
            pMV.maxRestrictSpeedScale = 0.5f;
        }
    }
    void FireSweep()
    {
        //Fires the sweep projectile
        GameObject sweep = Instantiate(sweepProjectile, sweepHitbox.position, Quaternion.LookRotation(new Vector3(1f, 0f, 0f) * (mv.playerIsRight ? 1f : -1f)));
        sweep.GetComponent<PenetrationProjectile>().player = player;
    }
    void SweepRecover()
    {
        mv.cd = false;
    }
    void Laser()
    {
        //Loads up the laser
        loadupaudio.Play();
        laserLoadup.Play();
        lookDir = (player.position - new Vector3(headLaserOrigin.position.x, headLaserOrigin.position.y, -2.5f)).normalized;
        laserAngle = Quaternion.LookRotation(lookDir);
        laserOrigin = new Vector3(headLaserOrigin.position.x, headLaserOrigin.position.y, -2.5f);
        //Fire tracer warning
        Instantiate(laserwarning, laserOrigin, laserAngle);
        Invoke("FireLaser", 0.7f);
    }
    void FireLaser()
    {
        //Fire actual laser
        laseraudio.Play();
        laserExplosion.Play();
        bool playerhit = false;
        //Since the laser is wide, use 2 parallel raycasts through the perpendicular ray vector
        Vector3 perp = Vector3.Cross(lookDir, Vector3.up).normalized;
        perp = new Vector3(perp.z, perp.y, perp.x);
        RaycastHit hit;
        //Check three parallel raycasts for player
        if (Physics.Raycast(laserOrigin, lookDir, out hit, Mathf.Infinity, groundPlayer))
        {
            Debug.Log(hit.collider.tag);
            if (hit.collider.tag == "Player")
            {
                playerhit = true;
            }
        }
        if (Physics.Raycast(laserOrigin + 0.75f * perp, lookDir, out hit, Mathf.Infinity, groundPlayer))
        {
            Debug.Log(hit.collider.tag);
            if (hit.collider.tag == "Player")
            {
                playerhit = true;
            }
        }
        if (Physics.Raycast(laserOrigin - 0.75f * perp, lookDir, out hit, Mathf.Infinity, groundPlayer))
        {
            Debug.Log(hit.collider.tag);
            if (hit.collider.tag == "Player")
            {
                playerhit = true;
            }
        }
        if (playerhit)
        {
            pH.Damage(laserDamage);
        }
        anim.SetTrigger("LaserFire");
        mv.cd = false;
        Instantiate(laser, laserOrigin, laserAngle);
    }
    void SuccesiveFire()
    {
        //Fires spreadshot multiple times
        lookDir = (player.position - new Vector3(headLaserOrigin.position.x, headLaserOrigin.position.y, -2.5f)).normalized;
        Invoke("RegainControl", 2.3f);
        for (int i = 0; i < 5; i++)
        {
            Invoke("SpreadShot", i * 0.5f);
        }
    }
    void SpreadShot()
    {
        ssPE.Play();
        anim.SetTrigger("LaserFire");
        spreadshotSE.Play();

        float offset = Random.Range(-.25f, .25f);
        for (int i = 0; i < 4; i++)
        {
            Vector3 newDir = new Vector3(lookDir.x, lookDir.y + (float)(i - 1) * 0.25f + offset, lookDir.z);
            Quaternion dir = Quaternion.LookRotation(newDir);
            dir.eulerAngles = new Vector3(dir.eulerAngles.x, newDir.x > 0 ? 90f : 270f, 0f);
            //Make sure the projectiles fire straight in the x axis.
            GameObject proj = Instantiate(spreadShotProj, new Vector3(headLaserOrigin.position.x, headLaserOrigin.position.y, -2.5f), dir);
            //Set the player attribute of the projectile to player to destroy it when out of range.

            proj.GetComponent<BasicProjectile>().player = player;
        }
    }
    void RegainControl()
    {
        mv.cd = false;
    }
    void Swipe()
    {
        Invoke("SwipeSound", 0.5f);
        esPE.Play();
        anim.SetTrigger("Swipe");
        Invoke("SwipeDamage", 0.9f);
        Invoke("RegainControl", 1.5f);
    }
    void SwipeSound()
    {
        swipeAudio.Play();
    }
    void SwipeDamage()
    {
        //Check swipe hitbox for player
        if (Physics.CheckBox(swipeHitbox.position, swipehitbox, Quaternion.identity, playerLayer))
        {
            pH.Damage(swipeDamage);
            pMV.AddVelocity((mv.playerIsRight ? 1f : -1f) * new Vector3(1f, 0f, 0f) * sweepForce);
            pMV.CeaseControl();
            pMV.maxRestrictSpeedScale = 0.5f;
        }
    }
}
