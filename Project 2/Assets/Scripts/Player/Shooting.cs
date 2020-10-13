using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public Vector3 mouseWorld;
    public LookAtMouse lm;
    public GameObject tracerRenderer;
    public GameObject explosion;
    public Transform barrelEnd;
    public float distance = 50f;
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
        Vector3 targetDir = (mouseWorld - adjustedBarrelEnd);
        Vector3 target = adjustedBarrelEnd + targetDir * distance;
        GameObject tracer = Instantiate(tracerRenderer, barrelEnd.position, Quaternion.identity);
        LineRenderer lr = tracer.GetComponent<LineRenderer>();

        RaycastHit hit;
        if (Physics.Raycast(adjustedBarrelEnd, targetDir, out hit, distance))
        {
            if (hit.transform.tag == "Enemy")
            {
                hit.transform.gameObject.GetComponent<HealthSystem>().Damage(10f);
            }
            target = hit.point;
        }

        lr.SetPosition(0, adjustedBarrelEnd);
        lr.SetPosition(1, target);
        Destroy(tracer, 1f);
        GameObject ex = Instantiate(explosion, target, Quaternion.Euler(-targetDir));
        Destroy(ex, 0.5f);

    }
}
