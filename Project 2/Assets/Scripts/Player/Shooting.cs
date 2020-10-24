using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shooting : MonoBehaviour
{
    public Vector3 mouseWorld;
    public LookAtMouse lm;
    public GameObject tracerRenderer;
    public GameObject railgunTracer;
    public GameObject explosion;
    public Transform shootingOrigin;
    public float distance = 50f;
    public Text ammoText;
    public int maxAmmo = 30;
    private int ammo;
    public Transform barrelEnd;
    public Transform chest;
    public LayerMask layerMask;
    public LayerMask groundLayer;
    public float delayBetweenShots = 0.25f;
    public float reloadTime = 2f;
    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        timer = 0f;
        ammo = maxAmmo;
    }

    // Update is called once per frame
    void Update()
    {
        Shoot();
    }
    void Shoot()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (timer <= 0f && ammo != 0)
            {
                FireShotgun(); // change this to change gun
                ammo -= 1;
                if (ammo == 0)
                {
                    timer = reloadTime;
                }
                else
                {
                    timer = delayBetweenShots;
                }
            }
            
            
        }
        if (timer <= 0f && ammo == 0)
        {
            ammo = maxAmmo;
        }
        DelayBetweenShot();
        if (ammo != 0)
        {
            ammoText.text = ammo.ToString() + "/" + maxAmmo.ToString();
        }
        else
        {
            ammoText.text = "Reloading";
        }
    }
    void FireRifle()
    {
        mouseWorld = lm.mouseWorld;
        Vector3 adjustedBarrelEnd = new Vector3(barrelEnd.position.x, barrelEnd.position.y, -2.5f);
        Quaternion targetRot = chest.rotation;
        targetRot = Quaternion.Euler(new Vector3(targetRot.eulerAngles.x, lm.playerIsRight ? 270f : 90f, 0f));
        float hitDistance = distance;
        Vector3 targetDir = (targetRot * Vector3.forward).normalized;
        Vector3 target = shootingOrigin.position + targetDir * distance;
        GameObject tracer = Instantiate(tracerRenderer, shootingOrigin.position, Quaternion.identity);
        LineRenderer lr = tracer.GetComponent<LineRenderer>();

        RaycastHit hit;
        if (Physics.Raycast(shootingOrigin.position, targetDir, out hit, distance, layerMask))
        {
            if (hit.transform.tag == "Enemy")
            {
                hit.transform.gameObject.GetComponent<HealthSystem>().Damage(10f);
            }
            hitDistance = hit.distance;
            target = hit.point;
        }

        lr.SetPosition(0, adjustedBarrelEnd);
        lr.SetPosition(1, adjustedBarrelEnd + targetDir * (hitDistance - (barrelEnd.position - shootingOrigin.position).magnitude));
        Destroy(tracer, 1f);
        GameObject ex = Instantiate(explosion, target, Quaternion.Euler(-targetDir));
        Destroy(ex, 0.5f);
    }
    void FireShotgun()
    {
        mouseWorld = lm.mouseWorld;
        Vector3 adjustedBarrelEnd = new Vector3(barrelEnd.position.x, barrelEnd.position.y, -2.5f);
        Quaternion targetRot = chest.rotation;
        targetRot = Quaternion.Euler(new Vector3(targetRot.eulerAngles.x, lm.playerIsRight ? 270f : 90f, 0f));
        float hitDistance = distance;
        Vector3 targetDir = (targetRot * Vector3.forward).normalized;
        Vector3 target = shootingOrigin.position + targetDir * distance;
        

        for (int i = 0; i < 5; i++)
        {
            GameObject tracer = Instantiate(tracerRenderer, shootingOrigin.position, Quaternion.identity);
            LineRenderer lr = tracer.GetComponent<LineRenderer>();
            RaycastHit hit;
            Vector3 noisedTargetDir = new Vector3(targetDir.x, targetDir.y + Random.Range(-0.2f, 0.2f), targetDir.z);
            if (Physics.Raycast(shootingOrigin.position, noisedTargetDir, out hit, distance, layerMask))
            {
                if (hit.transform.tag == "Enemy")
                {
                    hit.transform.gameObject.GetComponent<HealthSystem>().Damage(10f);
                }
                hitDistance = hit.distance;
                target = hit.point;
            }

            lr.SetPosition(0, adjustedBarrelEnd);
            lr.SetPosition(1, adjustedBarrelEnd + noisedTargetDir * (hitDistance - (barrelEnd.position - shootingOrigin.position).magnitude));
            Destroy(tracer, 1f);
            GameObject ex = Instantiate(explosion, target, Quaternion.Euler(-targetDir));
            Destroy(ex, 0.5f);
        }
    }
    void FireSmg()
    {
        mouseWorld = lm.mouseWorld;
        Vector3 adjustedBarrelEnd = new Vector3(barrelEnd.position.x, barrelEnd.position.y, -2.5f);
        Quaternion targetRot = chest.rotation;
        targetRot = Quaternion.Euler(new Vector3(targetRot.eulerAngles.x, lm.playerIsRight ? 270f : 90f, 0f));
        float hitDistance = distance;
        Vector3 targetDir = (targetRot * Vector3.forward).normalized;
        Vector3 target = shootingOrigin.position + targetDir * distance;
        GameObject tracer = Instantiate(tracerRenderer, shootingOrigin.position, Quaternion.identity);
        LineRenderer lr = tracer.GetComponent<LineRenderer>();
        RaycastHit hit;
        Vector3 noisedTargetDir = new Vector3(targetDir.x, targetDir.y + Random.Range(-0.05f, 0.05f), targetDir.z);
        if (Physics.Raycast(shootingOrigin.position, noisedTargetDir, out hit, distance, layerMask))
        {
            if (hit.transform.tag == "Enemy")
            {
                hit.transform.gameObject.GetComponent<HealthSystem>().Damage(10f);
            }
            hitDistance = hit.distance;
            target = hit.point;
        }
        lr.SetPosition(0, adjustedBarrelEnd);
        lr.SetPosition(1, adjustedBarrelEnd + noisedTargetDir * (hitDistance - (barrelEnd.position - shootingOrigin.position).magnitude));
        Destroy(tracer, 1f);
        GameObject ex = Instantiate(explosion, target, Quaternion.Euler(-targetDir));
        Destroy(ex, 0.5f);
    }
    void FireRailgun()
    {
        mouseWorld = lm.mouseWorld;
        Vector3 adjustedBarrelEnd = new Vector3(barrelEnd.position.x, barrelEnd.position.y, -2.5f);
        Quaternion targetRot = chest.rotation;
        targetRot = Quaternion.Euler(new Vector3(targetRot.eulerAngles.x, lm.playerIsRight ? 270f : 90f, 0f));
        float hitDistance = distance;
        Vector3 targetDir = (targetRot * Vector3.forward).normalized;
        Vector3 target = shootingOrigin.position + targetDir * distance;
        GameObject tracer = Instantiate(railgunTracer, shootingOrigin.position, Quaternion.identity);
        LineRenderer lr = tracer.GetComponent<LineRenderer>();

        RaycastHit hit;
        RaycastHit[] enemyHits;
        float checkDistance;
        if (Physics.Raycast(shootingOrigin.position, targetDir, out hit, distance, groundLayer))
        {
            Debug.Log(hit.collider.gameObject.name);
            checkDistance = Mathf.Min((shootingOrigin.position - hit.point).magnitude, 40f);
        }
        else
        {
            checkDistance = 40f;
        }
        enemyHits = Physics.RaycastAll(shootingOrigin.position, targetDir, checkDistance, layerMask);
        if (enemyHits.Length != 0)
        {
            foreach (RaycastHit enemy in enemyHits)
            {
                if (enemy.transform.tag == "Enemy")
                {
                    enemy.transform.gameObject.GetComponent<HealthSystem>().Damage(100f);
                }
            }
        }
        hitDistance = checkDistance;
        target = hit.point;

        lr.SetPosition(0, adjustedBarrelEnd);
        lr.SetPosition(1, adjustedBarrelEnd + targetDir * (hitDistance - (barrelEnd.position - shootingOrigin.position).magnitude));
        Destroy(tracer, 1f);
        GameObject ex = Instantiate(explosion, target, Quaternion.Euler(-targetDir));
        Destroy(ex, 0.5f);
    }
    void DelayBetweenShot()
    {
        timer -= Time.deltaTime;
    }
}
