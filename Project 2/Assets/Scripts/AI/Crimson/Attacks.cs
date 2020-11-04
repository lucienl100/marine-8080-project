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
    public LayerMask playerLayer;
    public LayerMask groundPlayer;
    BossMovement mv;
    PlayerHealth pH;
    
    Movement pMV;

    public bool sweep;
    Vector3 hitboxsize = new Vector3(2.5f, 1f, 2.5f);
    Vector3 swipehitbox = new Vector3(3.8f, 3f, 3.8f);
    float sweepForce = 10f;
    
    public ParticleSystem sweepPE;

    public ParticleSystem laserLoadup;
    public ParticleSystem laserExplosion;
    public Quaternion laserAngle;
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
        if (Mathf.Abs(t.position.x - player.position.x) < 10f)
        {
            return Random.Range(0f, 0.75f);
        }
        else
        {
            return Random.Range(0.26f, 1f);
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

        if (Physics.CheckBox(sweepHitbox.position, hitboxsize, Quaternion.identity, playerLayer))
        {
            pH.Damage(25f);
            pMV.AddVelocity((mv.playerIsRight ? 1f : -1f) * new Vector3(1f, 0f, 0f) * sweepForce);
            pMV.CeaseControl();
            pMV.maxRestrictSpeedScale = 0.5f;
        }
    }
    void FireSweep()
    {
        GameObject sweep = Instantiate(sweepProjectile, sweepHitbox.position, Quaternion.LookRotation(new Vector3(1f, 0f, 0f) * (mv.playerIsRight ? 1f : -1f)));
        sweep.GetComponent<PenetrationProjectile>().player = player;
    }
    void SweepRecover()
    {
        mv.cd = false;
    }
    void Laser()
    {
        
        loadupaudio.Play();
        laserLoadup.Play();
        lookDir = (player.position - new Vector3(headLaserOrigin.position.x, headLaserOrigin.position.y, -2.5f)).normalized;
        laserAngle = Quaternion.LookRotation(lookDir);
        Instantiate(laserwarning, new Vector3(headLaserOrigin.position.x, headLaserOrigin.position.y, -2.5f), laserAngle);
        Invoke("FireLaser", 0.8f);
    }
    void FireLaser()
    {
        laseraudio.Play();
        laserExplosion.Play();
        bool playerhit = false;
        Vector3 perp = Vector3.Cross(lookDir, Vector3.up).normalized;
        RaycastHit hit;
        if (Physics.Raycast(headLaserOrigin.position, lookDir, out hit, Mathf.Infinity, groundPlayer))
        {
            Debug.Log(hit.collider.tag);
            if (hit.collider.tag == "Player")
            {
                playerhit = true;
            }
        }
        if (Physics.Raycast(headLaserOrigin.position + 0.75f * perp, lookDir, out hit, Mathf.Infinity, groundPlayer))
        {
            Debug.Log(hit.collider.tag);
            if (hit.collider.tag == "Player")
            {
                playerhit = true;
            }
        }
        if (Physics.Raycast(headLaserOrigin.position - 0.75f * perp, lookDir, out hit, Mathf.Infinity, groundPlayer))
        {
            Debug.Log(hit.collider.tag);
            if (hit.collider.tag == "Player")
            {
                playerhit = true;
            }
        }
        if (playerhit)
        {
            pH.Damage(40f);
        }
        anim.SetTrigger("LaserFire");
        mv.cd = false;
        Instantiate(laser, new Vector3(headLaserOrigin.position.x, headLaserOrigin.position.y, -2.5f), laserAngle);
    }
    void SuccesiveFire()
    {
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
        //Make sure the projectiles fire straight in the x axis.

        for (int i = 0; i < 4; i++)
        {
            Vector3 newDir = new Vector3(lookDir.x, lookDir.y + (float)(i - 1) * 0.25f + offset, lookDir.z);
            Quaternion dir = Quaternion.LookRotation(newDir);
            dir.eulerAngles = new Vector3(dir.eulerAngles.x, newDir.x > 0 ? 90f : 270f, 0f);
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
        if (Physics.CheckBox(swipeHitbox.position, swipehitbox, Quaternion.identity, playerLayer))
        {
            pH.Damage(30f);
            pMV.AddVelocity((mv.playerIsRight ? 1f : -1f) * new Vector3(1f, 0.5f, 0f) * sweepForce);
            pMV.CeaseControl();
            pMV.maxRestrictSpeedScale = 0.5f;
        }
    }
}
