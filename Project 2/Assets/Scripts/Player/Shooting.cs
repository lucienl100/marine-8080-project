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
    private float[] delayBetweenShots = new float[] { 0.2f, 1.5f, 0.15f, 1.5f };
    private float[] reloadTime = new float[] { 2f, 2.5f, 1.5f, 2.5f };
    private float timer;
    private bool[] guns = new bool[] { true, false, false, false };
    private bool[] enabledguns = new bool[] { true, false, false, false };
    public GameObject[] icons;
    private int[] maxAmmoA = new int[] { 30, 8, 45, 2 };
    private int[] ammoA = new int[] { 30, 8, 45, 2 };
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
        SwitchGun();
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload(ActiveGun());
        }
    }
    void Shoot()
    {
        int i = ActiveGun();
        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (timer <= 0f && ammoA[i] != 0)
            {
                FireGun(i);
                ammoA[i] -= 1;
                if (ammoA[i] == 0)
                {
                    timer = reloadTime[i];
                }
                else
                {
                    timer = delayBetweenShots[i];
                }
            }


        }
        if (timer <= 0f && ammoA[i] == 0)
        {
            ammoA[i] = maxAmmoA[i];
        }
        DelayBetweenShot();
        if (ammoA[i] != 0)
        {
            ammoText.text = ammoA[i].ToString() + "/" + maxAmmoA[i].ToString();
        }
        else
        {
            ammoText.text = "Reloading";
        }
    }
    void Reload(int i)
    {
        ammoA[i] = 0;
        timer = reloadTime[i];
    }
    int ActiveGun()
    {
        for (int i = 0; i < 4; i++)
        {
            if (guns[i] == true)
            {
                return i;
            }
        }
        return 0;
    }
    void FireGun(float i)
    {
        if (i == 0)
        {
            FireRifle();
        }
        else if (i == 1)
        {
            FireShotgun();
        }
        else if (i == 2)
        {
            FireSmg();
        }
        else
        {
            FireRailgun();
        }
    }
    void SwitchGun()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            guns[0] = true;
            ammo = maxAmmoA[0];
            for (int i = 1; i < 4; i++)
            {
                guns[i] = false;

            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && enabledguns[1] == true)
        {
            guns[1] = true;
            ammo = maxAmmoA[1];
            for (int i = 0; i < 4; i++)
            {
                if (i != 1)
                {
                    guns[i] = false;
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) && enabledguns[2] == true)
        {
            guns[2] = true;
            ammo = maxAmmoA[2];
            for (int i = 0; i < 4; i++)
            {
                if (i != 2)
                {
                    guns[i] = false;
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4) && enabledguns[3] == true)
        {
            Debug.Log("switched");
            guns[3] = true;
            ammo = maxAmmoA[3];
            for (int i = 0; i < 3; i++)
            {
                guns[i] = false;
            }
        }
    }
    public void EnableGun(int i)
    {
        Debug.Log("enabled" + i);
        enabledguns[i] = true;
        icons[i].GetComponent<Slider>().value = 0f;
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
        Vector3 targetDir = (targetRot * Vector3.forward).normalized;
        Vector3 target = shootingOrigin.position + targetDir * distance;
        float hitDistance;
        for (int i = 0; i < 5; i++)
        {
            GameObject tracer = Instantiate(tracerRenderer, shootingOrigin.position, Quaternion.identity);
            LineRenderer lr = tracer.GetComponent<LineRenderer>();
            RaycastHit hit;
            Vector3 noisedTargetDir = new Vector3(targetDir.x, targetDir.y + Random.Range(-0.2f, 0.2f), targetDir.z);
            if (Physics.Raycast(shootingOrigin.position, noisedTargetDir, out hit, 30f, layerMask))
            {
                if (hit.transform.tag == "Enemy")
                {
                    hit.transform.gameObject.GetComponent<HealthSystem>().Damage(10f);
                }
                hitDistance = hit.distance;
                target = hit.point;
            }
            else
            {
                hitDistance = 30f;
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
                    enemy.transform.gameObject.GetComponent<HealthSystem>().Damage(80f);
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
