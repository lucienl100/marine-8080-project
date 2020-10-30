﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBlast : MonoBehaviour
{
    Transform t;
    public float radius = 30;
    public ParticleSystem blast;
    public float cooldown = 15f;
    float timer;
    void Start()
    {
        timer = 0f;
    }
    // Update is called once per frame
    void Update()
    {
		if (Input.GetKeyDown(KeyCode.F) && timer <= 0f)
		{
            DestoryProjectiles();
            timer = cooldown;
		}
        else if (timer > 0f)
        {
            timer -= Time.deltaTime;
        }
    }

    void DestoryProjectiles()
	{
        blast.Play();
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
