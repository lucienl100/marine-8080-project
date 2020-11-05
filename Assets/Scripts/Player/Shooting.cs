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
    private float shotgunDist = 20f;
    private float smgDist = 28f;
    private float rifleDist = 35f;
    private float railDist = 35f;
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
    public bool[] guns = new bool[] { true, false, false, false };
    public bool[] enabledguns = new bool[] { true, false, false, false };
    public GameObject[] icons;
    public GameObject[] iconFlash;
    public GameObject[] gunModels;
    public AudioSource[] gunsounds;
    public AudioSource reloadsound;
    private int[] maxAmmoA = new int[] { 30, 8, 45, 2 };
    private int[] ammoA = new int[] { 30, 8, 45, 2 };
    float difficulty;
    int idx;
    // Start is called before the first frame update
    void Start()
    {
        idx = 0;
        difficulty = PlayerPrefs.GetFloat("difficulty");
        for (int i = 1; i < 4; i++)
        {
            if (PlayerPrefs.GetInt("guns" + i.ToString()) == 1)
            {
                EnableGun(i);
            }
        }
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
            reloadsound.Play();
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
                gunsounds[i].Play();
                ammoA[i] -= 1;
                if (ammoA[i] == 0)
                {
                    reloadsound.Play();
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
            idx = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && enabledguns[1] == true)
        {
            idx = 1;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) && enabledguns[2] == true)
        {
            idx = 2;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4) && enabledguns[3] == true)
        {
            idx = 3;
        }
        guns[idx] = true;
        ammo = maxAmmoA[idx];
        gunModels[idx].SetActive(true);
        for (int i = 0; i < 4; i++)
        {
            if (i != idx)
            {
                gunModels[i].SetActive(false);
                guns[i] = false;
            }
            

        }
    }
    public void EnableGun(int i)
    {
        Debug.Log("enabled" + i);
        iconFlash[i - 1].GetComponent<Flash>().FlashImage();
        enabledguns[i] = true;
        icons[i].GetComponent<Slider>().value = 0f;
    }
    void FireRifle()
    {
        mouseWorld = lm.mouseWorld;
        Vector3 adjustedBarrelEnd = new Vector3(barrelEnd.position.x, barrelEnd.position.y, -2.5f);
        Quaternion targetRot = chest.rotation;
        targetRot = Quaternion.Euler(new Vector3(targetRot.eulerAngles.x, lm.playerIsRight ? 270f : 90f, 0f));
        float hitDistance = rifleDist;
        Vector3 targetDir = (targetRot * Vector3.forward).normalized;
        Vector3 target = shootingOrigin.position + targetDir * rifleDist;
        GameObject tracer = Instantiate(tracerRenderer, shootingOrigin.position, Quaternion.identity);
        LineRenderer lr = tracer.GetComponent<LineRenderer>();

        RaycastHit hit;
        if (Physics.Raycast(shootingOrigin.position, targetDir, out hit, rifleDist, layerMask))
        {
            if (hit.transform.tag == "Enemy")
            {
                hit.transform.gameObject.GetComponent<HealthSystem>().Damage(10f / difficulty);
            }
            else if (hit.transform.tag == "Boss")
            {
                hit.transform.gameObject.GetComponent<HealthSystemBoss>().Damage(10f / difficulty);
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
        Vector3 target = shootingOrigin.position + targetDir * shotgunDist;
        float hitDistance;
        for (int i = 0; i < 8; i++)
        {
            GameObject tracer = Instantiate(tracerRenderer, shootingOrigin.position, Quaternion.identity);
            LineRenderer lr = tracer.GetComponent<LineRenderer>();
            RaycastHit hit;
            Vector3 noisedTargetDir = new Vector3(targetDir.x, targetDir.y + Random.Range(-0.2f, 0.2f), targetDir.z);
            if (Physics.Raycast(shootingOrigin.position, noisedTargetDir, out hit, shotgunDist, layerMask))
            {
                if (hit.transform.tag == "Enemy")
                {
                    hit.transform.gameObject.GetComponent<HealthSystem>().Damage(17f / difficulty);
                }
                else if (hit.transform.tag == "Boss")
                {
                    hit.transform.gameObject.GetComponent<HealthSystemBoss>().Damage(8f / difficulty);
                }
                hitDistance = hit.distance;
                target = hit.point;
            }
            else
            {
                hitDistance = shotgunDist;
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
        float hitDistance = smgDist;
        Vector3 targetDir = (targetRot * Vector3.forward).normalized;
        Vector3 target = shootingOrigin.position + targetDir * smgDist;
        GameObject tracer = Instantiate(tracerRenderer, shootingOrigin.position, Quaternion.identity);
        LineRenderer lr = tracer.GetComponent<LineRenderer>();
        RaycastHit hit;
        Vector3 noisedTargetDir = new Vector3(targetDir.x, targetDir.y + Random.Range(-0.05f, 0.05f), targetDir.z);
        if (Physics.Raycast(shootingOrigin.position, noisedTargetDir, out hit, smgDist, layerMask))
        {
            if (hit.transform.tag == "Enemy")
            {
                hit.transform.gameObject.GetComponent<HealthSystem>().Damage(9f / difficulty);
            }
            else if (hit.transform.tag == "Boss")
            {
                hit.transform.gameObject.GetComponent<HealthSystemBoss>().Damage(9f / difficulty);
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
        float hitDistance = railDist;
        Vector3 targetDir = (targetRot * Vector3.forward).normalized;
        Vector3 target = shootingOrigin.position + targetDir * railDist;
        GameObject tracer = Instantiate(railgunTracer, shootingOrigin.position, Quaternion.identity);
        LineRenderer lr = tracer.GetComponent<LineRenderer>();

        RaycastHit hit;
        RaycastHit[] enemyHits;
        float checkDistance;
        if (Physics.Raycast(shootingOrigin.position, targetDir, out hit, railDist, groundLayer))
        {
            checkDistance = Mathf.Min((shootingOrigin.position - hit.point).magnitude, railDist);
        }
        else
        {
            checkDistance = railDist;
        }
        enemyHits = Physics.RaycastAll(shootingOrigin.position, targetDir, checkDistance, layerMask);
        if (enemyHits.Length != 0)
        {
            foreach (RaycastHit enemy in enemyHits)
            {
                if (enemy.transform.tag == "Enemy")
                {
                    enemy.transform.gameObject.GetComponent<HealthSystem>().Damage(90f / difficulty);
                }
                else if (enemy.transform.tag == "Boss")
                {
                    enemy.transform.gameObject.GetComponent<HealthSystemBoss>().Damage(90f / difficulty);
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
