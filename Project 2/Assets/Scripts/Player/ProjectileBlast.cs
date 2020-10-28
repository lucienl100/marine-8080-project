using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBlast : MonoBehaviour
{
    Transform t;
    public float radius = 30;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
		if (Input.GetKeyDown(KeyCode.LeftShift))
		{
            DestoryProjectiles();
		}
    }

    void DestoryProjectiles()
	{
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach (var hitCollider in colliders)
		{
            if(hitCollider.gameObject.GetComponent<IProjectile>() != null)
			{
                Destroy(hitCollider.gameObject);
			}
		}
	}
}
