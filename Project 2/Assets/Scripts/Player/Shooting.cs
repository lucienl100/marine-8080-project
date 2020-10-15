using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public Vector3 mouseWorld;
    public LookAtMouse lm;
    public GameObject tracerRenderer;
    public GameObject explosion;
    public Transform shootingOrigin;
    public float distance = 50f;
    public Transform barrelEnd;
    public Transform chest;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Shoot();
    }
    void Shoot()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Fire();
        }
    }
    void Fire()
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
        if (Physics.Raycast(shootingOrigin.position, targetDir, out hit, distance))
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
}
